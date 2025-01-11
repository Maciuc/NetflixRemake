<template>
  <div class="movie-edit">
    <h2>Edit Movie</h2>
    <form @submit.prevent="submitForm" class="form">
      <div class="form-group">
        <label for="name">Name:</label>
        <input id="name" v-model="movie.Name" required />
      </div>
      <div class="form-group">
        <label for="description">Description:</label>
        <textarea
          id="description"
          v-model="movie.Description"
          rows="3"
        ></textarea>
      </div>
      <div class="form-group">
        <label for="videoPath">Image URL:</label>
        <input id="videoPath" v-model="movie.VideoPath" />
      </div>
      <div class="form-group">
        <label for="startPrice">Start Price:</label>
        <input
          id="startPrice"
          v-model.number="movie.StartPrice"
          type="number"
          required
        />
      </div>
      <button type="submit" class="form-btn">Update</button>
    </form>
  </div>
</template>

<script>
import MovieService from "@/services/MovieService";

export default {
  props: {
    Id: {
      type: Number,
      required: true,
    },
  },
  data() {
    return {
      movie: {
        Name: "",
        Description: "",
        VideoPath: "",
        StartPrice: 0,
      },
      loading: true,
    };
  },
  methods: {
    async fetchMovie() {
      try {
        this.movie = await MovieService.getMovie(this.Id); // Apelează endpoint-ul `getMovie`
        this.loading = false;
      } catch (error) {
        console.error("Failed to fetch movie:", error);
      }
    },
    async submitForm() {
      try {
        await MovieService.updateMovie({
          Id: this.Id,
          ...this.movie,
        });
        alert("Movie updated successfully!");
        this.$router.push("/"); // Redirecționează la pagina principală
      } catch (error) {
        console.error("Failed to update movie:", error);
      }
    },
  },
  created() {
    this.fetchMovie(); // Încarcă detaliile filmului când componenta este creată
  },
};
</script>

<style scoped>
.movie-edit {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  background: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

h2 {
  text-align: center;
  margin-bottom: 20px;
  color: #333;
}

.form {
  display: flex;
  flex-direction: column;
}

.form-group {
  margin-bottom: 15px;
}

label {
  font-weight: bold;
  margin-bottom: 5px;
  display: block;
}

input,
textarea {
  width: 100%;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
}

textarea {
  resize: vertical;
}

.form-btn {
  background-color: #007bff;
  color: white;
  border: none;
  padding: 10px 15px;
  cursor: pointer;
  font-size: 16px;
  border-radius: 4px;
  transition: background-color 0.3s ease;
}

.form-btn:hover {
  background-color: #0056b3;
}
</style>
