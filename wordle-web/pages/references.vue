<template>
  <v-container>
    <v-row class="text-center mb-6">
      <v-col cols="12">
        <h2 class="display-2">References</h2>
      </v-col>
    </v-row>
    
    <!-- Add Reference Button -->
    <v-btn small color="primary" class="rounded-circle mb-4" @click="showAddReferenceDialog = true">
      <v-icon>mdi-plus</v-icon>
    </v-btn>

    <!-- Add Reference Dialog -->
    <v-dialog v-model="showAddReferenceDialog" max-width="600px">
      <v-card>
        <v-card-title>Add Reference</v-card-title>
        <v-card-text>
          <v-text-field v-model="newReference.name" label="Name"></v-text-field>
          <v-text-field v-model="newReference.contact" label="Contact"></v-text-field>
          <v-text-field v-model="newReference.relation" label="Relation"></v-text-field>
        </v-card-text>
        <v-card-actions>
          <v-btn color="blue darken-1" text @click="addReference">Add</v-btn>
          <v-btn text @click="showAddReferenceDialog = false">Cancel</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
    
    <!-- References -->
    <v-card class="mb-4" v-for="(reference, index) in references" :key="index">
      <v-card-title>{{ reference.name }}</v-card-title>
      <v-card-subtitle>{{ reference.contact }}</v-card-subtitle>
      <v-card-text>{{ reference.relation }}</v-card-text>
      <v-card-actions>
        <v-btn icon @click="removeReference(reference.id)">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import Axios from 'axios';

interface Reference {
  id: number;
  name: string;
  contact: string;
  relation: string;
}

const references = ref<Reference[]>([]);
const showAddReferenceDialog = ref(false);
const newReference = ref<Reference>({
  name: '',
  contact: '',
  relation: ''
});

const fetchReferences = async () => {
  try {
    const response = await Axios.get('/api/references');
    references.value = response.data;
  } catch (error) {
    console.error('Error fetching references:', error);
  }
};

const addReference = async () => {
  try {
    const response = await Axios.post('/api/references', newReference.value);
    references.value.push(response.data);
    newReference.value = { name: '', contact: '', relation: '' }; // Clear inputs
    showAddReferenceDialog.value = false; // Close dialog
  } catch (error) {
    console.error('Error adding reference:', error);
  }
};

const removeReference = async (id: number) => {
  try {
    await Axios.delete(`/api/references/${id}`);
    references.value = references.value.filter(ref => ref.id !== id);
  } catch (error) {
    console.error('Error removing reference:', error);
  }
};

onMounted(fetchReferences);
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
</style>

