<template>
  <div class="h-screen w-screen bg-gradient-to-r from-purple-400 via-blue-500 to-pink-500 bg-[length:200%_200%] animate-gradient flex relative">
    <LeftPanel :stations="stations"
               @select-station="loadTracks"
               @add-station="showAddModal = true" />

    <div class="flex-grow flex flex-col justify-center items-center relative">
      <transition name="fade">
        <MainPanel v-if="showAddModal"
                   @close="showAddModal = false"
                   @add="addStation"
                   class="animate-slide-up" />
      </transition>
      <p v-if="!showAddModal && selectedStationId === null"
         class="text-gray-100 text-center text-sm absolute bottom-4">
        Выберите станцию или создайте новую.
      </p>
    </div>

    <transition name="slide-right">
      <RightPanel v-if="selectedStationId !== null"
                  :tracks="tracks"
                  @add-track="showAddTrackModal = true"
                  @delete-track="deleteTrack"
                  @close="selectedStationId = null"
                  @play-track="playTrack" />
    </transition>

    <transition name="fade">
      <AddTrackPanel v-if="showAddTrackModal"
                     @close="showAddTrackModal = false"
                     @add="addTrack"
                     class="animate-slide-up" />
    </transition>

    <transition name="fade">
      <div v-if="currentTrack" class="absolute bottom-4 left-4 right-4 bg-white shadow-lg p-4 rounded-md">
        <div class="flex items-center">
          <div class="flex-1">
            <p class="font-semibold">{{ currentTrack.title }}</p>
            <p class="text-sm text-gray-500">{{ currentTrack.artist }}</p>
          </div>
          <audio ref="audio" :src="currentTrack.fileUrl" @ended="stopTrack" autoplay />
          <button class="ml-4 bg-red-500 text-white px-3 py-1 rounded hover:bg-red-700"
                  @click="stopTrack">
            ✖
          </button>
          <button class="ml-4 bg-blue-500 text-white px-3 py-1 rounded hover:bg-blue-700"
                  @click="togglePause">
            {{ isPaused ? '▶' : '❚❚' }}
          </button>
          <input type="range" min="0" max="1" step="0.01"
                 v-model="volume" @input="updateVolume"
                 class="ml-4 w-20" />
        </div>
        <div class="mt-2">
          <input type="range"
                 :max="audioDuration"
                 v-model="audioCurrentTime"
                 @input="updateAudioTime"
                 class="w-full" />
          <div class="text-sm text-gray-500 mt-1">
            {{ formatTime(audioCurrentTime) }} / {{ formatTime(audioDuration) }}
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
  import axios from "axios";
  import LeftPanel from "./components/LeftPanel.vue";
  import MainPanel from "./components/MainPanel.vue";
  import RightPanel from "./components/RightPanel.vue";
  import AddTrackPanel from "./components/AddTrackModal.vue";

  export default {
    components: { LeftPanel, MainPanel, RightPanel, AddTrackPanel },
    data() {
      return {
        stations: [],
        tracks: [],
        selectedStationId: null,
        showAddModal: false,
        showAddTrackModal: false,
        currentTrack: null,
        audioCurrentTime: 0,
        audioDuration: 0,
        isPaused: false,
        volume: 1,
      };
    },
    methods: {
      async loadStations() {
        try {
          const response = await axios.get("/api/Stations");
          this.stations = response.data;
        } catch (error) {
          console.error("Ошибка загрузки станций:", error);
        }
      },
      async addStation(name) {
        try {
          const response = await axios.post("/api/Stations", { name });
          this.stations.push(response.data);
          this.showAddModal = false;
        } catch (error) {
          console.error("Ошибка добавления станции:", error);
        }
      },
      async loadTracks(stationId) {
        this.selectedStationId = stationId;
        try {
          const response = await axios.get(`/api/Tracks/${stationId}`);
          this.tracks = response.data;
        } catch (error) {
          console.error("Ошибка загрузки треков:", error);
        }
      },
      async addTrack(trackData) {
        const formData = new FormData();
        formData.append("file", trackData.file);
        formData.append("title", trackData.title);
        formData.append("artist", trackData.artist);
        formData.append("stationId", this.selectedStationId);

        try {
          const response = await axios.post("/api/Tracks", formData, {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          });
          this.tracks.push(response.data);
          this.showAddTrackModal = false;
        } catch (error) {
          console.error("Ошибка добавления трека:", error);
        }
      },
      async deleteTrack(trackId) {
        try {
          await axios.delete(`/api/Tracks/${trackId}`);
          this.tracks = this.tracks.filter((track) => track.id !== trackId);
        } catch (error) {
          console.error("Ошибка удаления трека:", error);
        }
      },
      playTrack(track) {
        this.currentTrack = track;
        this.$nextTick(() => {
          const audio = this.$refs.audio;
          audio.play();
          audio.addEventListener("timeupdate", this.syncAudioTime);
          this.audioDuration = audio.duration || 0;
        });
      },
      stopTrack() {
        const audio = this.$refs.audio;
        if (audio) {
          audio.pause();
          audio.removeEventListener("timeupdate", this.syncAudioTime);
        }
        this.currentTrack = null;
      },
      togglePause() {
        const audio = this.$refs.audio;
        if (audio) {
          if (this.isPaused) {
            audio.play();
          } else {
            audio.pause();
          }
          this.isPaused = !this.isPaused;
        }
      },
      syncAudioTime() {
        const audio = this.$refs.audio;
        this.audioCurrentTime = audio.currentTime;
        this.audioDuration = audio.duration;
      },
      updateAudioTime() {
        const audio = this.$refs.audio;
        audio.currentTime = this.audioCurrentTime;
      },
      updateVolume() {
        const audio = this.$refs.audio;
        if (audio) {
          audio.volume = this.volume;
        }
      },
      formatTime(seconds) {
        const minutes = Math.floor(seconds / 60);
        const secs = Math.floor(seconds % 60);
        return `${minutes}:${secs.toString().padStart(2, "0")}`;
      },
    },
    created() {
      this.loadStations();
    },
  };
</script>

<style scoped>
  .fade-enter-active,
  .fade-leave-active {
    transition: opacity 0.5s ease;
  }

  .fade-enter-from,
  .fade-leave-to {
    opacity: 0;
  }

  .slide-right-enter-active,
  .slide-right-leave-active {
    transition: transform 0.5s ease, opacity 0.5s ease;
  }

  .slide-right-enter-from {
    transform: translateX(100%);
    opacity: 0;
  }

  .slide-right-leave-to {
    transform: translateX(100%);
    opacity: 0;
  }

  .right-panel {
    background-color: #1a202c;
    color: white;
  }

  .track-slide {
    animation: slide-down 0.5s ease-out;
  }

  .audio-controls {
    display: flex;
    align-items: center;
  }
</style>
