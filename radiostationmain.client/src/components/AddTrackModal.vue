<template>
  <div class="absolute top-0 left-0 w-full h-full bg-black bg-opacity-50 flex justify-center items-center">
    <div class="bg-white p-6 rounded-md w-96">
      <h2 class="text-xl font-semibold mb-4">Добавить трек</h2>
      <form @submit.prevent="handleAdd">
        <div class="mb-4">
          <label for="title" class="block text-sm font-semibold">Название</label>
          <input v-model="track.title" id="title" type="text" class="w-full p-2 mt-2 border border-gray-300 rounded" required />
        </div>
        <div class="mb-4">
          <label for="artist" class="block text-sm font-semibold">Исполнитель</label>
          <input v-model="track.artist" id="artist" type="text" class="w-full p-2 mt-2 border border-gray-300 rounded" required />
        </div>
        <div class="mb-4">
          <label for="file" class="block text-sm font-semibold">Файл</label>
          <input @change="handleFileChange" id="file" type="file" class="w-full p-2 mt-2 border border-gray-300 rounded" required />
        </div>
        <div class="flex justify-between mt-4">
          <button @click="$emit('close')" class="text-gray-600 hover:text-gray-800">Отменить</button>
          <button type="submit" class="bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600">Добавить</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
  export default {
    data() {
      return {
        track: {
          title: '',
          artist: '',
          file: null,
        },
      };
    },
    methods: {
      handleFileChange(event) {
        this.track.file = event.target.files[0];
      },
      handleAdd() {
        this.$emit('add', this.track);
      },
    },
  };
</script>

<style scoped>
  /* Custom styling */
</style>
