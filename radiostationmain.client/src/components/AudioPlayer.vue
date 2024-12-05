<template>
  <div class="fixed bottom-0 left-0 w-full bg-gray-900 text-white p-4 shadow-lg flex items-center">
    <button @click="$emit('close')" class="text-red-500 mr-4">
      ✖
    </button>
    <div class="flex-grow">
      <p class="text-lg font-bold">{{ track.title }}</p>
      <p class="text-sm text-gray-400">{{ track.artist }}</p>
      <input type="range" v-model="currentTime" :max="duration" @input="seekTrack" class="w-full mt-2" />
    </div>
    <div class="ml-4 flex items-center">
      <input type="range" v-model="volume" min="0" max="1" step="0.1" class="w-24 mr-2" />
      <button @click="togglePlayPause" class="bg-blue-500 p-2 rounded">
        {{ isPlaying ? '⏸ Pause' : '▶ Play' }}
      </button>
    </div>
  </div>
</template>

<script>
  export default {
    props: ["track"],
    data() {
      return {
        audio: null,
        isPlaying: false,
        currentTime: 0,
        duration: 0,
        volume: 1,
      };
    },
    watch: {
      volume(newVolume) {
        if (this.audio) this.audio.volume = newVolume;
      },
    },
    methods: {
      togglePlayPause() {
        if (this.isPlaying) {
          this.audio.pause();
        } else {
          this.audio.play();
        }
        this.isPlaying = !this.isPlaying;
      },
      seekTrack() {
        if (this.audio) this.audio.currentTime = this.currentTime;
      },
    },
    mounted() {
      this.audio = new Audio(this.track.fileUrl);
      this.audio.addEventListener("loadedmetadata", () => {
        this.duration = this.audio.duration;
      });
      this.audio.addEventListener("timeupdate", () => {
        this.currentTime = this.audio.currentTime;
      });
    },
    beforeUnmount() {
      if (this.audio) {
        this.audio.pause();
        this.audio = null;
      }
    },
  };
</script>
