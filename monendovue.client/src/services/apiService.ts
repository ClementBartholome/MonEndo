import axios from 'axios';

const API_URL = 'https://localhost:7206/';

const apiService = {
    async getDonneesCarnetSante(carnetSanteId: number) {
        try {
            const response = await axios.get(`${API_URL}CarnetSante/${carnetSanteId}`);

            if (response.status === 200) {
                return response.data;
            } else {
                throw new Error('Get failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    },

    async getLastDonneesCarnetSante(carnetSanteId: number) {
        try {
            const response = await axios.get(`${API_URL}CarnetSante/lastentries/${carnetSanteId}`);

            if (response.status === 200) {
                return response.data;
            } else {
                throw new Error('Get failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    },
    
    async postDonneesDouleurs(donneesDouleurs: any) {
        try {
            const response = await axios.post(`${API_URL}DonneesDouleurs`, donneesDouleurs, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 201) {
                console.log(response.data);
                return response.data;
            } else {
                throw new Error('Post failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    },
    
    async postDonneesActivitePhysique(donneesActivitePhysique: any) {
        try {
            const response = await axios.post(`${API_URL}DonneesActivitePhysique`, donneesActivitePhysique, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 201) {
                console.log(response.data);
                return response.data;
            } else {
                throw new Error('Post failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    },
    
    async deleteDonneesActivitePhysique(donneesActivitePhysiqueId: number) {
        try {
            const response = await axios.delete(`${API_URL}DonneesActivitePhysique/${donneesActivitePhysiqueId}`);

            if (response.status === 204) {
                console.log(response.data);
                return response.data;
            } else {
                throw new Error('Delete failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    },
    
    async deleteDonneesDouleurs(donneesDouleursId: number) {
        try {
            const response = await axios.delete(`${API_URL}DonneesDouleurs/${donneesDouleursId}`);

            if (response.status === 204) {
                console.log(response.data);
                return response.data;
            } else {
                throw new Error('Delete failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    },
    
    async deleteDonneesMedicament(donneesMedicamentId: number) {
        try {
            const response = await axios.delete(`${API_URL}DonneesMedicament/${donneesMedicamentId}`);

            if (response.status === 204) {
                console.log(response.data);
                return response.data;
            } else {
                throw new Error('Delete failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    },
    
    async postMedicament(donneesMedicament: any) {
        try {
            const response = await axios.post(`${API_URL}Medicament`, donneesMedicament, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 201) {
                console.log(response.data);
                return response.data;
            } else {
                throw new Error('Post failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    },
    
    async postDonneesPriseMedicament(donneesPriseMedicament: any) {
        try {
            const response = await axios.post(`${API_URL}DonneesMedicament`, donneesPriseMedicament, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 201) {
                console.log(response.data);
                return response.data;
            } else {
                throw new Error('Post failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    },
    
    async putDonneesMedicament(medicamentId: number, donneesMedicament: any) {
        try {
            const response = await axios.put(`${API_URL}Medicament/${medicamentId}`, donneesMedicament, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (response.status === 200) {
                console.log(response.data);
                return response.data;
            } else {
                throw new Error('Put failed');
            }
        } catch (error: any) {
            console.error(error);
            return null;
        }
    }
}

export default apiService;