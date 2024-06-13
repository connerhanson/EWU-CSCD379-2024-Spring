<template>
  <v-container>
    <v-row class="text-center mb-6">
      <v-col cols="12">
        <h2 class="display-2">Education</h2>
      </v-col>
    </v-row>
    
    <!-- Add Education Button -->
    <v-btn small color="primary" class="rounded-circle mb-4" @click="showAddEducationDialog = true">
      <v-icon>mdi-plus</v-icon>
    </v-btn>

    <!-- Add Education Dialog -->
    <v-dialog v-model="showAddEducationDialog" max-width="600px">
      <v-card>
        <v-card-title>Add Education</v-card-title>
        <v-card-text>
          <v-text-field v-model="newEducation.schoolName" label="School Name"></v-text-field>
          <v-text-field v-model="newEducation.datesAttended" label="Dates Attended"></v-text-field>
          <v-textarea v-model="newEducation.accomplishment" label="Accomplishment / Degree"></v-textarea>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="addEducation">Add</v-btn>
          <v-btn text @click="showAddEducationDialog = false">Cancel</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Educations -->
    <v-card class="mb-4" v-for="(education, index) in educations" :key="index">
      <v-card-title>{{ education.schoolName }}</v-card-title>
      <v-card-subtitle>{{ education.datesAttended }}</v-card-subtitle>
      <v-expand-transition>
        <v-card-text>
          <p>{{ education.accomplishment }}</p>
        </v-card-text>
      </v-expand-transition>
      <v-card-actions>
        <v-btn icon @click="removeEducation(education.id)">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import Axios from 'axios';

interface Education {
  id: number;
  schoolName: string;
  datesAttended: string;
  accomplishment: string;
}

const educations = ref<Education[]>([]);

const showAddEducationDialog = ref(false);
const newEducation = ref<Education>({
  id: 0,
  schoolName: '',
  datesAttended: '',
  accomplishment: ''
});

// Fetch education entries from API on component mount
onMounted(async () => {
  await fetchEducations();
});

const fetchEducations = async () => {
  try {
    const response = await Axios.get('/api/education/GetEducations');
    educations.value = response.data;
  } catch (error) {
    console.error('Error fetching educations:', error);
  }
};

const addEducation = async () => {
  try {
    const response = await Axios.post('/api/education/PostEducation', newEducation.value);
    educations.value.push(response.data); // Assuming API returns the added education with ID
    newEducation.value = { id: 0, schoolName: '', datesAttended: '', accomplishment: '' }; // Clear inputs
    showAddEducationDialog.value = false; // Close dialog
  } catch (error) {
    console.error('Error adding education:', error);
  }
};

const removeEducation = async (id: number) => {
  try {
    await Axios.delete(`/api/education/${id}`);
    educations.value = educations.value.filter(edu => edu.id !== id);
  } catch (error) {
    console.error('Error removing education:', error);
  }
};
</script>

<style scoped>
/* Center text and add margin-bottom for spacing */
.text-center {
  text-align: center;
}

.mb-6 {
  margin-bottom: 3rem; /* Adjust as needed for spacing */
}

/* Circular button */
.rounded-circle {
  border-radius: 50%;
  width: 40px;
  height: 40px;
  min-width: 40px;
  min-height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Adjustments for card titles */
.v-card-title {
  font-size: 1.5rem;
  font-weight: bold;
}

/* Adjustments for card subtitles */
.v-card-subtitle {
  font-size: 1rem;
  color: grey;
}

/* Expand transition for accomplishment */
.v-expand-transition {
  max-height: 300px;
  overflow: hidden;
}
</style>

