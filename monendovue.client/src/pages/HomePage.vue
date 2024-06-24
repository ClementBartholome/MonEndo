<template>
  <Carnet @show-pain-form="showPainForm" @show-activite-form="showActiviteForm" v-if="carnetVisible" />
  <PainForm v-if="painFormVisible" @close="showPainForm" />
  <ActiviteForm v-if="activiteFormVisible" @close="showActiviteForm" />
</template>

<script setup lang="ts">
import {ref} from 'vue'
import PainForm from "@/components/PainForm.vue";
import Carnet from "@/components/Carnet.vue";
import ActiviteForm from "@/components/ActiviteForm.vue";

const carnetVisible = ref(true)
const painFormVisible = ref(false)
const activiteFormVisible = ref(false)

const showPainForm = () => {
  painFormVisible.value = !painFormVisible.value
  carnetVisible.value = !carnetVisible.value
}

const showActiviteForm = () => {
  activiteFormVisible.value = !activiteFormVisible.value
  carnetVisible.value = !carnetVisible.value
}

</script>

<style scoped>

</style>