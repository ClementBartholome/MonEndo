import {createRouter, createWebHistory} from 'vue-router'
import HomePage from '@/pages/HomePage.vue'
import Schedule from '@/pages/Schedule.vue'
import LoginPage from "@/pages/LoginPage.vue";
import {useAuthStore} from "@/store/auth";
import PainPage from "@/pages/PainPage.vue";
import ActivitePage from "@/pages/ActivitePage.vue";
import MedicamentPage from "@/pages/MedicamentPage.vue";
import TransitPage from "@/pages/TransitPage.vue";

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
                    next({name: 'login'});
                } else {
                    next();
                }
            }
        },
        {
            path: '/douleurs',
            name: 'douleurs',
            component: PainPage,
        },
        {
            path: '/activite',
            name: 'activite',
            component: ActivitePage,
        },
        {
            path: '/transit',
            name: 'transit',
            component: TransitPage,
        },
        {
            path: '/medicaments',
            name: 'medicaments',
            component: MedicamentPage,
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
