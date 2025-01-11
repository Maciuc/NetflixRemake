import axios from "axios";

const API_URL = "http://localhost:7260/api/Movie"; // Schimbă portul dacă e necesar

export default {
  async getMovies() {
    const response = await axios.get(`${API_URL}/getMovies`);
    return response.data;
  },
  // async getMovie(id) {
  //   const response = await axios.get(`${API_URL}/getMovie?id=${id}`);
  //   return response.data;
  // },
  async getMovie(id) {
    const response = await axios.get(`${API_BASE_URL}/getMovie`, {
      params: { id },
    });
    return response.data;
  },
  async addMovie(movie) {
    const response = await axios.post(`${API_URL}/addMovie`, movie);
    return response.data;
  },
  // async updateMovie(movie) {
  //   const response = await axios.put(`${API_URL}/updateMovie`, movie);
  //   return response.data;
  // },
  async updateMovie(movie) {
    await axios.put(`${API_BASE_URL}/updateMovie`, movie);
  },
  async deleteMovie(id) {
    const response = await axios.delete(`${API_URL}/deleteMovie?id=${id}`);
    return response.data;
  },
};
