import axios from 'axios';

const googleApiService = {
    async getAuthorizationUrl(userId: string) {
        const response = await axios.get(`https://monendoapi.azurewebsites.net/GoogleApi/authenticate?userId=${userId}`);
        return response.data;
    },
    async getUpcomingEvents(accessToken) {
        const response = await axios.get('https://www.googleapis.com/calendar/v3/calendars/primary/events', {
            headers: {
                Authorization: `Bearer ${accessToken}`
            },
            params: {
                maxResults: 4,
                singleEvents: true,
                orderBy: 'startTime',
                timeMin: new Date().toISOString(),
            }
        });

        return response.data.items.filter(event => event.summary.includes('MED'));
    },
    async refreshAccessToken(userId: string, refreshToken: string) {
        const response = await axios.get(`https://monendoapi.azurewebsites.net/GoogleApi/refreshToken?userId=${userId}&refreshToken=${refreshToken}`);
        return response.data;
    }
}

export default googleApiService;