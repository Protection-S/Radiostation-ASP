

<template>
  <div class="right-panel h-full w-1/3 bg-gray-800 text-white shadow-lg p-4 relative">
    <button class="absolute top-4 right-4 bg-red-500 hover:bg-red-700 text-white font-bold py-1 px-2 rounded"
            @click="$emit('close')">
      ✖
    </button>

    <h2 class="text-lg font-semibold mb-4">Треки</h2>
    <ul>
      <li v-for="track in tracks"
          :key="track.id"
          class="flex justify-between items-center p-2 hover:bg-gray-700 rounded cursor-pointer">
        <span @click="$emit('play-track', track)">
          {{ track.title }} - {{ track.artist }}
        </span>
        <button @click="$emit('delete-track', track.id)"
                class="text-red-500 hover:text-red-700">
          Удалить
        </button>
      </li>
    </ul>

    <button class="mt-4 bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
            @click="$emit('add-track')">
      Добавить трек
    </button>
  </div>
</template>

<script>
  export default {
    props: {
      tracks: {
        type: Array,
        required: true,
      },
    },
  };
</script>

<style scoped>
  .right-panel {
    transition: transform 0.5s ease, opacity 0.5s ease;
  }
</style>
