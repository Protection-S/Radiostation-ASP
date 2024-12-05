<template>
  <div class="absolute bg-white rounded-lg shadow-lg p-6 w-96 flex flex-col gap-4">
    <h2 class="text-lg font-semibold text-gray-800">Добавить станцию</h2>

    <input v-model="stationName"
           class="border border-gray-300 rounded-md p-2 w-full"
           placeholder="Введите название станции" />

    <p v-if="errorMessage" class="text-red-500 text-sm">{{ errorMessage }}</p>

    <div class="flex justify-end gap-2">
      <button class="bg-gray-300 text-gray-800 px-4 py-2 rounded-md hover:bg-gray-400 transition"
              @click="$emit('close')">
        Отмена
      </button>
      <button class="bg-blue-500 text-white px-4 py-2 rounded-md hover:bg-blue-600 transition"
              @click="submit">
        Добавить
      </button>
    </div>
  </div>
</template>

<script>
  export default {
    data() {
      return {
        stationName: "",
        errorMessage: "",
      };
    },
    methods: {
      submit() {
        if (this.stationName.trim() === "") {
          this.errorMessage = "Название станции не может быть пустым!";
          return;
        }

        this.$emit("add", this.stationName.trim());
        this.stationName = "";
        this.errorMessage = "";
      },
    },
  };
</script>
