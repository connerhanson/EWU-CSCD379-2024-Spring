<template>
  <v-app>
    <v-container>
      <v-row justify="center">
        <v-col cols="12" class="text-center">
          <v-avatar size="200" class="avatar" @click="triggerFileInput">
            <v-img :src="avatarUrl" alt="Avatar"></v-img>
          </v-avatar>
          <input
            type="file"
            ref="fileInput"
            accept="image/*"
            style="display: none"
            @change="onFileChange"
          />
        </v-col>
      </v-row>
      <v-row justify="center">
        <v-col cols="12" md="4">
          <!-- Name -->
          <div class="name-card">
            <span class="name-text" v-if="!editingName" @click="editingName = true">{{ name || 'Your Name' }}</span>
            <input
              v-model="name"
              v-if="editingName"
              type="text"
              @keyup.enter="saveName"
              @blur="saveName"
              class="name-input"
              placeholder="Enter your name"
            >
          </div>
          <!-- Current Position -->
          <div class="position-card">
            <span class="position-text" v-if="!editingPosition" @click="editingPosition = true">{{ currentPosition || 'Current Position' }}</span>
            <input
              v-model="currentPosition"
              v-if="editingPosition"
              type="text"
              @keyup.enter="savePosition"
              @blur="savePosition"
              class="position-input"
              placeholder="Enter your current position"
            >
          </div>
          <!-- Location -->
          <div class="location-card">
            <span class="location-text" v-if="!editingLocation" @click="editingLocation = true">{{ location || 'Location' }}</span>
            <input
              v-model="location"
              v-if="editingLocation"
              type="text"
              @keyup.enter="saveLocation"
              @blur="saveLocation"
              class="location-input"
              placeholder="Enter your location"
            >
          </div>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12" md="4">
          <v-card>
            <v-card-title>About Me</v-card-title>
            <v-card-text>
              <v-textarea label="About Me" v-model="aboutMe" outlined @blur="saveProfile"></v-textarea>
            </v-card-text>
          </v-card>
        </v-col>
        <v-col cols="12" md="8">
          <v-card>
            <v-card-title>Skills</v-card-title>
            <v-card-text>
              <v-chip
                v-for="(skill, index) in skills"
                :key="index"
                @click="removeSkill(index)"
                class="skill-chip"
              >
                {{ skill }}
              </v-chip>
              <v-btn icon @click="openDialog">
                <v-icon>mdi-plus</v-icon>
              </v-btn>
              <v-dialog v-model="dialog" max-width="500px">
                <v-card>
                  <v-card-title>
                    <span class="headline">Add a new skill</span>
                  </v-card-title>
                  <v-card-text>
                    <v-text-field
                      v-model="newSkill"
                      label="New Skill"
                      @keyup.enter="addSkill"
                      outlined
                    ></v-text-field>
                  </v-card-text>
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="blue darken-1" text @click="closeDialog">Cancel</v-btn>
                    <v-btn color="blue darken-1" text @click="addSkill">Add</v-btn>
                  </v-card-actions>
                </v-card>
              </v-dialog>
            </v-card-text>
          </v-card>

          <!-- Navigation Box -->
          <v-card class="mt-4">
            <v-card-title>Navigation</v-card-title>
            <v-card-actions class="d-flex justify-center">
              <v-btn color="primary" text @click="navigateTo('expirience')">Experience</v-btn>
              <v-btn color="primary" text @click="navigateTo('skills')">Skills</v-btn>
              <v-btn color="primary" text @click="navigateTo('references')">References</v-btn>
              <v-btn color="primary" text @click="navigateTo('education')">Education</v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-app>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import Axios from 'axios';

const router = useRouter();

const name = ref('');
const editingName = ref(false);
const aboutMe = ref('');
const skills = ref<string[]>([]);
const newSkill = ref('');
const avatarUrl = ref('https://via.placeholder.com/150');
const avatarFile = ref<File | null>(null); // Store the file
const fileInput = ref<HTMLInputElement | null>(null);
const currentPosition = ref('');
const editingPosition = ref(false);
const location = ref('');
const editingLocation = ref(false);
const dialog = ref(false); // Control the dialog

function triggerFileInput() {
  fileInput.value?.click();
}

function onFileChange(event: Event) {
  const target = event.target as HTMLInputElement;
  const file = target.files?.[0];
  if (file) {
    avatarFile.value = file;
    const reader = new FileReader();
    reader.onload = (e) => {
      avatarUrl.value = e.target?.result as string;
      saveProfile(); // Save profile after changing the avatar
    };
    reader.readAsDataURL(file);
  }
}

function saveName() {
  editingName.value = false;
  saveProfile();
}

function savePosition() {
  editingPosition.value = false;
  saveProfile();
}

function saveLocation() {
  editingLocation.value = false;
  saveProfile();
}

function openDialog() {
  dialog.value = true;
}

function closeDialog() {
  dialog.value = false;
}

function addSkill() {
  if (newSkill.value && !skills.value.includes(newSkill.value)) {
    skills.value.push(newSkill.value);
    newSkill.value = '';
    saveProfile();
  }
  closeDialog();
}

function removeSkill(index: number) {
  skills.value.splice(index, 1);
  saveProfile();
}

async function fetchProfile() {
  try {
    const response = await Axios.get('/api/UserProfile/1');
    const profile = response.data;
    name.value = profile.name;
    currentPosition.value = profile.currentPosition;
    location.value = profile.location;
    aboutMe.value = profile.aboutMe;
    skills.value = JSON.parse(profile.skills); // Parse the JSON string to an array
    avatarUrl.value = profile.avatarUrl;
  } catch (error) {
    console.error('Error fetching profile:', error);
  }
}

async function saveProfile() {
  try {
    const formData = new FormData();
    formData.append('id', '1'); // Assuming the ID is 1 for this example
    formData.append('name', name.value);
    formData.append('currentPosition', currentPosition.value);
    formData.append('location', location.value);
    formData.append('aboutMe', aboutMe.value);
    formData.append('skills', JSON.stringify(skills.value)); // Convert array to JSON string
    if (avatarFile.value) {
      formData.append('avatarUrl', avatarFile.value);
    }

    const response = await Axios.post('/api/UserProfile/Profile', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });
    console.log('Profile saved successfully', response);
  } catch (error) {
    console.error('Error saving profile:', error);
  }
}

function navigateTo(route: string) {
  router.push(`/${route}`);
}

onMounted(() => {
  fetchProfile();
});
</script>

<style>
.name-card, .position-card, .location-card {
  text-align: center;
  padding: 8px; /* Optional padding for spacing */
}

.name-text, .position-text, .location-text {
  font-size: 1.2rem; /* Optional font size adjustment */
  cursor: pointer;
}

.name-input, .position-input, .location-input {
  text-align: center;
  border: none;
  outline: none;
  font-size: 1.2rem; /* Optional font size adjustment */
  padding: 4px; /* Optional padding for spacing */
}

.position-text {
  font-size: 1rem; /* Adjusted smaller font size for position and location */
}

.location-text {
  font-size: 0.8rem;
}

.v-textarea {
  min-height: 200px;
}

.skill-chip {
  cursor: pointer;
  margin-right: 4px; /* Optional margin for spacing */
  margin-bottom: 4px; /* Optional margin for spacing */
}
</style>

