<script setup lang="ts">

import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger
} from "@/components/ui/dropdown-menu";

import { useAuthStore} from "@/store/auth";
import { onMounted } from "vue";
import router from "@/router";

const auth = useAuthStore();

const user = auth.getUser();

onMounted(() => {
  auth.checkAuth();
  console.log(user)
  let userName = user?.email;
  console.log(userName);
});

const handleLogout = async () => {
  await auth.logout();
  router.push('/login');
}
</script>

<template>
  <header>
    <nav class="navbar-side flex items-center justify-between border-b shadow-lg mb-3">
      <a href="#">
        <img class="logo" src="../images/MonEndo_transparent.png" alt="">
      </a>
      <div class="flex items-center justify-between mb-auto">
        <ul class="flex-grow">
          <li class="flex items-center" :class="{ active: $route.path === '/' }">
            <a href="/" class="flex flex-col items-center text-dark w-full">
              <span class="material-symbols-outlined">clinical_notes</span>
              <span>Carnet</span>
            </a>
          </li>
          <li class="flex items-center" :class="{ active: $route.path === '/agenda' }">
            <a href="/agenda" class="flex flex-col items-center text-dark w-full">
              <span class="material-symbols-outlined">calendar_month</span>
              <span>Rendez-vous</span>
            </a>
          </li>
        </ul>
      </div>
    </nav>

    <div class="user-navbar flex items-center justify-end fixed top-0 right-0 my-6 mx-12 gap-4">
      <a href="#" class="flex items-center text-dark">
        <span class="material-symbols-outlined">notifications</span>
      </a>
      <span class="material-symbols-outlined">account_circle</span>
      <div class="relative inline-block text-left">
        <span>{{ user?.email }}</span>
        <DropdownMenu>
          <DropdownMenuTrigger>
            <span class="material-symbols-outlined">arrow_drop_down</span>
          </DropdownMenuTrigger>
          <DropdownMenuContent>
            <DropdownMenuItem>
              <a href="#">Profil</a>
            </DropdownMenuItem>
            <DropdownMenuItem>
              <a href="#">Paramètres</a>
            </DropdownMenuItem>
            <DropdownMenuSeparator/>
            <DropdownMenuItem>
              <button @click="handleLogout">Déconnexion</button>
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </div>
  </header>
</template>

<style scoped>

</style>