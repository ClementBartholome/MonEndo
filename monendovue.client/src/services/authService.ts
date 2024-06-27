import axios from 'axios';

const API_URL = 'https://monendoapi.azurewebsites.net/';

const authService = {
    async login(email: string, password: string) {
        try {
            const response = await axios.post(`${API_URL}Account/login?email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`, null, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 200) {
                return response.data;
            } else {
                throw new Error('Login failed');
            }
        } catch (error: any) {
            console.error(error);
            if (error.response) {
                console.error(error.response.data);
            }
            return null;
        }
    },
    
    async register(email: string, password: string) {
        try {
            const response = await axios.post(`${API_URL}Account/register`, {
                email,
                password
            }, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 201) {
                return response.data;
            } else {
                throw new Error('Register failed');
            }
        } catch (error) {
            console.error(error);
            return null;
        }
    },
    
    async logout() {
        try {
            await axios.post(`${API_URL}Account/logout`, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        } catch (error) {
            console.error(error);
        }
    }
};

export default authService;