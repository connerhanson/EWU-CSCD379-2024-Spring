<template>
  <v-container>
    <v-row class="text-center mb-6">
      <v-col cols="12">
        <h2 class="display-2">Skills</h2>
      </v-col>
    </v-row>
    
    <!-- Add Skill Button -->
    <v-btn small color="primary" class="rounded-circle mb-4" @click="showAddSkillDialog = true">
      <v-icon>mdi-plus</v-icon>
    </v-btn>

    <!-- Add Skill Dialog -->
    <v-dialog v-model="showAddSkillDialog" max-width="600px">
      <v-card>
        <v-card-title>Add Skill</v-card-title>
        <v-card-text>
          <v-text-field v-model="newSkill.skillName" label="Skill Name"></v-text-field>
          <v-select v-model="newSkill.skillLevel" :items="skillLevels" label="Skill Level"></v-select>
          <v-textarea v-model="newSkill.skillDescription" label="Description of Skill"></v-textarea>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="addSkill">Add</v-btn>
          <v-btn text @click="showAddSkillDialog = false">Cancel</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- Skills -->
    <v-card class="mb-4" v-for="(skill, index) in skills" :key="skill.id">
      <v-card-title>{{ skill.skillName }}</v-card-title>
      <v-card-subtitle>{{ skill.skillLevel }}</v-card-subtitle>
      <v-expand-transition>
        <v-card-text>
          <p>{{ skill.skillDescription }}</p>
        </v-card-text>
      </v-expand-transition>
      <v-card-actions>
        <v-btn icon @click="removeSkill(skill.id)">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import Axios from 'axios';

interface Skill {
  id: number;
  skillName: string;
  skillLevel: string;
  skillDescription: string;
}

const skills = ref<Skill[]>([]);
const showAddSkillDialog = ref(false);
const newSkill = ref<Skill>({
  id: 0,
  skillName: '',
  skillLevel: '',
  skillDescription: ''
});

const skillLevels = [
  'Beginner',
  'Intermediate',
  'Advanced',
  'Expert'
];

const fetchSkills = async () => {
  try {
    const response = await Axios.get('/api/skills');
    skills.value = response.data;
  } catch (error) {
    console.error('Error fetching skills:', error);
  }
};

const addSkill = async () => {
  try {
    const response = await Axios.post('/api/skills', newSkill.value);
    skills.value.push(response.data); // Assuming API returns the added skill with ID
    newSkill.value = { id: 0, skillName: '', skillLevel: '', skillDescription: '' }; // Clear inputs
    showAddSkillDialog.value = false; // Close dialog
  } catch (error) {
    console.error('Error adding skill:', error);
  }
};

const removeSkill = async (id: number) => {
  try {
    await Axios.delete(`/api/skills/${id}`);
    skills.value = skills.value.filter(skill => skill.id !== id);
  } catch (error) {
    console.error('Error removing skill:', error);
  }
};

onMounted(fetchSkills); // Fetch skills on component mount
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

/* Expand transition for skill description */
.v-expand-transition {
  max-height: 300px;
  overflow: hidden;
}
</style>

