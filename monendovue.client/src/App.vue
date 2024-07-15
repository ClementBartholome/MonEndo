<template>
  <Layout v-if="auth.user"/>
  <transition name="fade" mode="out-in">
    <RouterView/>
  </transition>
  <Toaster/>
</template>

<script setup lang="ts">
import {RouterView} from 'vue-router';
import Toaster from '@/components/ui/toast/Toaster.vue'
import Layout from "@/components/Layout.vue";
import {useAuthStore} from '@/store/auth';
import {onMounted} from 'vue';

const auth = useAuthStore();

onMounted(() => {
  auth.checkAuth();
});
</script>

<style>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.7s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>