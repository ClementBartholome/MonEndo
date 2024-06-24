<template>
  <Layout v-if="auth.user"/>
  <RouterView/>
  <Toaster />
</template>

<script setup lang="ts">
import {RouterView} from 'vue-router';
import Toaster from '@/components/ui/toast/Toaster.vue'
import Layout from "@/components/Layout.vue";
import { useAuthStore } from '@/store/auth';
import { onMounted } from 'vue';

const auth = useAuthStore();

onMounted(() => {
  auth.checkAuth();
});
</script>