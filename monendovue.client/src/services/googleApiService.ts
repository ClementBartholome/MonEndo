﻿import axios from 'axios';

const googleApiService = {
    async getAuthorizationUrl(userId: string) {
        const response = await axios.get(`https://localhost:7206/GoogleApi/authenticate?userId=${userId}`);
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

        return response.data.items;
    },
    async refreshAccessToken(userId: string, refreshToken: string) {
        const response = await axios.get(`https://localhost:7206/GoogleApi/refreshToken?userId=${userId}&refreshToken=${refreshToken}`);
        return response.data;
    }
}

export default googleApiService;