<template>
  <div class="movie-list">
    <h1>Movie Management</h1>
    <button class="refresh-btn" @click="fetchMovies">Refresh Movies</button>
    <MovieForm @movie-added="fetchMovies" />
    <div v-if="movies.length" class="movies-grid">
      <div class="movie-card" v-for="movie in movies" :key="movie.Id">
        <img
          v-if="movie.VideoPath"
          :src="movie.VideoPath"
          alt="Movie Poster"
          class="movie-image"
        />
        <h2 class="movie-title">{{ movie.Name }}</h2>
        <p class="movie-description">{{ movie.Description }}</p>
        <p class="movie-price">
          <strong>Price:</strong> ${{ movie.StartPrice }}
        </p>
        <button class="edit-btn" @click="editMovie(movie.Id)">Edit</button>
        <button class="delete-btn" @click="deleteMovie(movie.Id)">
          Delete
        </button>
      </div>
    </div>
    <p v-else class="no-movies">No movies available.</p>
  </div>
</template>

<script>
import MovieService from "@/services/MovieService";
import MovieForm from "@/components/MovieForm.vue";
import { useRouter } from "vue-router";

export default {
  components: { MovieForm },
  setup() {
    const router = useRouter();
    return { router };
  },
  data() {
    return {
      movies: [],
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
    editMovie(id) {
      this.router.push(`/edit/${id}`); // Navigare către pagina de editare
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
  max-width: 1200px;
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
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 20px;
  margin-top: 20px;
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
  width: 100%;
  height: auto;
  max-height: 200px;
  object-fit: cover;
  margin-bottom: 15px;
  border-bottom: 1px solid #ddd;
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
}

.no-movies {
  text-align: center;
  font-size: 18px;
  color: #7f8c8d;
  margin-top: 20px;
}
</style>
