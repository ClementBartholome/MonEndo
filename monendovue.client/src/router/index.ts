import { createRouter, createWebHistory } from 'vue-router'
import HomePage from '@/pages/HomePage.vue'
import Schedule from '@/pages/Schedule.vue'
import LoginPage from "@/pages/LoginPage.vue";
import {useAuthStore} from "@/store/auth";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'home',
            component: HomePage,
            beforeEnter: (to, from, next) => {
                const authStore = useAuthStore();
                if (!authStore.token) {
                    next({ name: 'login' });
                } else {
                    next();
                }
            }
        },
        {
            path: '/agenda',
            name: 'agenda',
            component: Schedule
        },
        {
            path: '/login',
            name: 'login',
            component: LoginPage
        }
    ]
})

export default router
