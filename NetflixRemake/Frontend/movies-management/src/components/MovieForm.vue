<template>
  <div class="movie-form">
    <h2>{{ isEdit ? "Edit Movie" : "Add New Movie" }}</h2>
    <form @submit.prevent="submitForm" class="form">
      <div class="form-group">
        <label for="name">Name:</label>
        <input id="name" v-model="movie.Name" required />
      </div>
      <div class="form-group">
        <label for="Description">Description:</label>
        <textarea
          id="Description"
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
      <button type="submit" class="form-btn">
        {{ isEdit ? "Edit" : "Add" }}
      </button>
    </form>
  </div>
</template>

<script>
import MovieService from "@/services/MovieService";

export default {
  props: {
    Id: Number,
  },
  data() {
    return {
      movie: {
        Name: "",
        Description: "",
        VideoPath: "",
        StartPrice: 0,
      },
    };
  },
  computed: {
    isEdit() {
      return !!this.Id; // Verifică dacă este o acțiune de editare
    },
  },
  methods: {
    async fetchMovie() {
      if (this.isEdit) {
        // Preia valorile din `query` (pentru consistență)
        const { query } = this.$route;
        this.movie.Name = query.Name || "";
        this.movie.Description = query.Description || "";
        this.movie.VideoPath = query.VdeoPath || "";
        this.movie.StartPrice = query.StartPrice || 0;
      }
    },
    async submitForm() {
      if (this.isEdit) {
        await MovieService.updateMovie({
          ...this.movie,
          Id: this.Id,
        });
      } else {
        await MovieService.addMovie(this.movie);
        this.resetForm();
        this.$emit("movie-added");
      }
      this.$router.push("/"); // Redirecționează la lista de filme
    },
    resetForm() {
      this.movie = {
        Name: "",
        Description: "",
        VideoPath: "",
        StartPrice: 0,
      };
    },
  },
  created() {
    this.fetchMovie();
  },
};
</script>

<style scoped>
.movie-form {
  padding: 20px;
  max-width: 600px;
  margin: 0 auto;
  background: #f9f9f9;
  border: 1px solid #ddd;
  border-radius: 10px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

h2 {
  text-align: center;
  color: #2c3e50;
  margin-bottom: 20px;
}

.form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

label {
  font-weight: bold;
  margin-bottom: 5px;
  color: #34495e;
}

input,
textarea {
  padding: 10px;
  font-size: 14px;
  border: 1px solid #ccc;
  border-radius: 5px;
}

input:focus,
textarea:focus {
  border-color: #3498db;
  outline: none;
  box-shadow: 0 0 5px rgba(52, 152, 219, 0.5);
}

textarea {
  resize: none;
}

.form-btn {
  padding: 10px 15px;
  font-size: 16px;
  color: white;
  background-color: #27ae60;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  text-align: center;
}

.form-btn:hover {
  background-color: #2ecc71;
}
</style>
