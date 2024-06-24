import { defineStore } from 'pinia';
import authService from '@/services/authService';
import type { User } from '@/models/user';

export const useAuthStore = defineStore({
    id: 'auth',
    state: (): { user: User | null, token: string | null } => ({
        user: null,
        token: null,
    }),
    actions: {
        async login(email: string, password: string) {
            const response = await authService.login(email, password);
            if (response) {
                this.user = {
                    email: response.userName,
                    carnetSanteId: response.carnetSanteId
                };
                this.token = response.token;
                localStorage.setItem('token', response.token);
                const user: User = {
                    email: response.userName,
                    carnetSanteId: response.carnetSanteId
                };
                localStorage.setItem('token', response.token);
                localStorage.setItem('user', JSON.stringify(user));
            }
            return response;
        },
        async register(email: string, password: string) {
            const response = await authService.register(email, password);
            if (response) {
                this.user = response.user;
                this.token = response.token;
                localStorage.setItem('token', response.token);
                const user: User = {
                    email: response.userName,
                    carnetSanteId: response.carnetSanteId
                };
                localStorage.setItem('user', JSON.stringify(user));
            }
            return response;
        },
        async logout() {
            await authService.logout();
            this.user = null;
            this.token = null;
            localStorage.removeItem('token');
            localStorage.removeItem('user');
        },
        checkAuth() {
            const token = localStorage.getItem('token');
            const userItem = localStorage.getItem('user');
            if (token && userItem) {
                this.token = token;
                this.user = JSON.parse(userItem);
            }
        },
        getUser() {
            return this.user;
        }
    },
});