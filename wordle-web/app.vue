<template>
  <NuxtLayout>
    <v-app>
      <v-app-bar color="primary" dense :elevation="2">
        <v-app-bar-title>Resume</v-app-bar-title>
        <v-spacer></v-spacer>
        <v-btn @click="router.push('/')">Home</v-btn>
        <v-btn icon="mdi-theme-light-dark" @click="toggleTheme"/>
        <div class="switch-container">
          <v-switch v-model="editMode" hide-details></v-switch>
          <div class="switch-label">{{ editMode ? 'Edit' : 'View' }}</div>
        </div>
      </v-app-bar>
      <v-main>
        <NuxtPage :isEdit="editMode" />
      </v-main>
    </v-app>
  </NuxtLayout>
</template>

<script setup lang="ts">
import { useRouter } from "vue-router";
import { useTheme } from "vuetify";
import nuxtStorage from "nuxt-storage";

const router = useRouter();
const theme = useTheme();
let editMode = ref(false);



function toggleTheme() {
  if (theme.global.name.value === "light") {
    theme.global.name.value = "dark";
  } else {
    theme.global.name.value = "light";
  }

  nuxtStorage.localStorage.setData("theme", theme.global.name.value);
}

// Set editMode to false (View mode) by default
editMode.value = false;
</script>

<style scoped>
.switch-container {
  display: flex;
  align-items: center;
  margin-left: auto; /* Pushes the switch container to the right */
  padding-right: 24px; /* Adds some padding to align with app bar edges */
}

.switch-label {
  font-size: 12px; /* Adjust label font size */
  color: #fff;
  margin-left: 8px; /* Add some space between switch and label */
}
</style>

