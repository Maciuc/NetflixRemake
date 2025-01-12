<template>
  <div class="movie-list">
    <h1>Movie Management</h1>
    <button class="refresh-btn" @click="fetchMovies">Refresh Movies</button>
    <div v-if="movies.length" class="movies-grid">
      <div
        class="movie-card"
        v-for="movie in movies"
        :key="movie.Id"
        @click="incrementViews(movie)"
      >
        <img
          :src="$utils.ShowDynamicImage(movie.VideoPath, 'default-img')"
          @error="$utils.ShowImageUserNotFoundOnServer"
          alt=""
          class="movie-image"
        />
        <h2 class="movie-title">{{ movie.Name }}</h2>
        <p class="movie-description">{{ movie.Description }}</p>
        <p class="movie-description">
          <strong>Views: {{ movie.Views }}</strong>
        </p>
        <p class="movie-price">
          <strong>Start price:</strong> ${{ movie.StartPrice }}
        </p>
        <p class="movie-price">
          <strong>Current price:</strong> ${{ movie.CurrentPrice }}
        </p>
        <button class="edit-btn" @click="openEditModal(movie)">Edit</button>
        <button class="delete-btn" @click="deleteMovie(movie.Id)">
          Delete
        </button>
      </div>
    </div>
    <p v-else class="no-movies">No movies available.</p>

    <!-- Edit Modal -->
    <div v-if="isEditModalOpen" class="modal-overlay">
      <div class="modal-content">
        <h2>Edit Movie</h2>
        <form @submit.prevent="submitEditForm" class="form">
          <div class="form-group">
            <label for="name">Name:</label>
            <input id="name" v-model="selectedMovie.Name" required />
          </div>
          <div class="form-group">
            <label for="description">Description:</label>
            <textarea
              id="description"
              v-model="selectedMovie.Description"
              rows="3"
            ></textarea>
          </div>
          <div class="form-group">
            <label for="imageBase64">Image:</label>
            <input
              id="imageBase64"
              type="file"
              @change="handleEditFileUpload"
              accept="image/*"
            />
            <small v-if="selectedMovie.VideoPath"
              >Image selected successfully!</small
            >
          </div>
          <div class="form-group">
            <label for="startPrice">Start Price:</label>
            <input
              id="startPrice"
              v-model.number="selectedMovie.StartPrice"
              type="number"
              required
            />
          </div>
          <div class="form-group">
            <label for="viewsValue">Views value:</label>
            <input
              id="viewsValue"
              v-model.number="selectedMovie.ViewsValue"
              type="number"
            />
          </div>
          <div class="form-group">
            <label for="startPrice">Views threshold:</label>
            <input
              id="viewsThreshold"
              v-model.number="selectedMovie.ViewsThreshold"
              type="number"
            />
          </div>
          <div class="modal-actions">
            <button type="submit" class="form-btn">Save</button>
            <button type="button" class="cancel-btn" @click="closeEditModal">
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import MovieService from "@/services/MovieService";
import { useRouter } from "vue-router";

export default {
  setup() {
    const router = useRouter();
    return { router };
  },
  data() {
    return {
      movies: [],
      isEditModalOpen: false,
      selectedMovie: null, // Filmul selectat pentru editare
    };
  },
  methods: {
    async fetchMovies() {
      this.movies = await MovieService.getMovies();
    },
    async deleteMovie(id) {
      await MovieService.deleteMovie(id);
      this.fetchMovies();
    },
    openEditModal(movie) {
      this.selectedMovie = { ...movie }; // Copiem datele filmului selectat
      this.isEditModalOpen = true;
    },
    closeEditModal() {
      this.isEditModalOpen = false;
      this.selectedMovie = null;
    },
    async submitEditForm() {
      try {
        await MovieService.updateMovie(this.selectedMovie);
        alert("Movie updated successfully!");
        this.closeEditModal();
        this.fetchMovies(); // Actualizează lista după editare
      } catch (error) {
        console.error("Failed to update movie:", error);
      }
    },
    async incrementViews(movie) {
      try {
        movie.Views += 1;
        await MovieService.addView(movie.Id); // Actualizăm filmul în backend
      } catch (error) {
        console.error("Failed to increment views:", error);
      }
    },
    handleEditFileUpload(event) {
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          this.selectedMovie.ImageBase64 = e.target.result; // Salvează conținutul Base64
        };
        reader.readAsDataURL(file); // Citește fișierul ca Base64
      }
    },
  },
  created() {
    this.fetchMovies();
  },
};
</script>

<style scoped>
.movie-list {
  padding: 20px;
  max-width: 1600px;
  margin: 0 auto;
}

h1 {
  text-align: center;
  color: #2c3e50;
}

.refresh-btn {
  display: block;
  margin: 10px auto;
  padding: 10px 20px;
  font-size: 16px;
  background-color: #3498db;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.refresh-btn:hover {
  background-color: #2980b9;
}

.movies-grid {
  display: grid;
  grid-template-columns: repeat(
    auto-fit,
    minmax(300px, 1fr)
  ); /* Grila va avea max. 3 carduri pe rând */
  gap: 20px;
  justify-content: center; /* Centrăm cardurile dacă sunt mai puține pe rând */
}

.movie-card {
  background: #f9f9f9;
  border: 1px solid #ddd;
  border-radius: 10px;
  overflow: hidden;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  text-align: center;
  padding: 15px;
}

.movie-image {
  width: 100%; /* Asigurăm că imaginea ocupă toată lățimea containerului */
  max-width: 250px; /* Limităm lățimea maximă a imaginii */
  height: 150px; /* Setăm o înălțime fixă pentru imagini */
  object-fit: cover; /* Asigurăm că imaginile sunt tăiate pentru a se potrivi în container */
  margin: 0 auto 15px; /* Centrăm imaginea și adăugăm spațiu jos */
  border-bottom: 1px solid #ddd;
}

.movie-card:active {
  transform: scale(0.98);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
}

.movie-title {
  font-size: 18px;
  font-weight: bold;
  color: #333;
  margin: 10px 0;
}

.movie-description {
  font-size: 14px;
  color: #666;
  margin: 10px 0;
}

.movie-price {
  font-size: 16px;
  font-weight: bold;
  color: #27ae60;
  margin: 10px 0;
}

.delete-btn {
  margin-top: 10px;
  padding: 8px 15px;
  font-size: 14px;
  background-color: #e74c3c;
  color: white;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.delete-btn:hover {
  background-color: #c0392b;
  transform: scale(1.05);
}

.no-movies {
  text-align: center;
  font-size: 18px;
  color: #7f8c8d;
  margin-top: 20px;
}

.movies-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 20px;
}

.movie-card {
  padding: 15px;
  background: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  text-align: center;
}

.movie-image {
  width: 100%;
  height: auto;
  border-radius: 4px;
}

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  max-width: 500px;
  width: 100%;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

.modal-actions {
  display: flex;
  justify-content: space-between;
  margin-top: 15px;
}

.form-btn {
  background-color: #007bff;
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.form-btn:hover {
  background-color: #0056b3;
}

.cancel-btn {
  background-color: #dc3545;
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.cancel-btn:hover {
  background-color: #a71d2a;
}

.edit-btn {
  background-color: #3498db; /* Verde pentru editare */
  color: white;
  padding: 8px 15px;
  margin-right: 10px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: background-color 0.3s ease, transform 0.2s ease;
}

.edit-btn:hover {
  background-color: #0056b3; /* Verde mai închis la hover */
  transform: scale(1.05); /* Mărire ușoară la hover */
}

.edit-btn:active {
  background-color: #1e7e34; /* Verde și mai închis la click */
  transform: scale(1); /* Eliminare mărire la click */
}

.modal-content .form-group {
  margin-bottom: 15px;
}

.modal-content label {
  display: block;
  font-weight: bold;
  margin-bottom: 5px;
  color: #333; /* Text întunecat */
}

.modal-content input,
.modal-content textarea {
  width: 100%;
  padding: 10px;
  font-size: 14px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
  outline: none;
  transition: border-color 0.3s ease, box-shadow 0.3s ease;
}

.modal-content input:focus,
.modal-content textarea:focus {
  border-color: #007bff; /* Albastru pentru câmp focus */
  box-shadow: 0 0 4px rgba(0, 123, 255, 0.5);
}

.modal-content textarea {
  resize: vertical; /* Permite redimensionarea doar pe verticală */
}

.modal-actions .form-btn,
.modal-actions .cancel-btn {
  width: calc(50% - 5px); /* Asigură spațiu egal pentru cele două butoane */
}

.modal-actions .form-btn {
  margin-right: 10px;
}

.modal-actions .cancel-btn {
  margin-left: 10px;
}
</style>
