<template>
  <div class="w-1/4 bg-gray-800 p-4 text-white">
    <h2 class="text-xl font-semibold">Радиостанции</h2>
    <ul>
      <li v-for="station in stations" :key="station.id" class="my-2">
        <button @click="$emit('select-station', station.id)" class="w-full text-left px-2 py-1 hover:bg-gray-600 rounded">
          {{ station.name }}
        </button>
      </li>
    </ul>
    <button @click="$emit('add-station')" class="w-full bg-blue-500 text-white py-1 mt-4 rounded hover:bg-blue-600">
      Добавить станцию
    </button>
  </div>
</template>

<script>
  export default {
    props: {
      stations: Array,
    },
  };
</script>

<style scoped>
</style>
