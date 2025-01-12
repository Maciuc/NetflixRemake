<template>
  <div class="movie-form">
    <h2>Add New Movie</h2>
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
        <label for="imageBase64">Image:</label>
        <input
          id="imageBase64"
          type="file"
          @change="handleFileUpload"
          accept="image/*"
        />
        <small v-if="movie.ImageBase64">Image selected successfully!</small>
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
      <div class="form-group">
        <label for="startPrice">Views value:</label>
        <input
          id="viewsValue"
          v-model.number="movie.ViewsValue"
          type="number"
          required
        />
      </div>
      <div class="form-group">
        <label for="startPrice">Views Threshold:</label>
        <input
          id="viewsThreshold"
          v-model.number="movie.ViewsThreshold"
          type="number"
          required
        />
      </div>
      <button type="submit" class="form-btn">Add</button>
    </form>
  </div>
</template>

<script>
import MovieService from "@/services/MovieService";

export default {
  data() {
    return {
      movie: {
        Name: "",
        Description: "",
        VideoPath: "",
        StartPrice: 0,
        ViewsValue: 0,
        ViewsThreshold: 0,
      },
    };
  },
  methods: {
    async submitForm() {
      try {
        await MovieService.addMovie(this.movie);
        alert("Movie added successfully!");
        this.resetForm();
        //this.$emit("movie-added"); // Emetem evenimentul pentru actualizarea listei
      } catch (error) {
        console.error("Failed to add movie:", error);
      }
    },
    resetForm() {
      this.movie = {
        Name: "",
        Description: "",
        VideoPath: "",
        StartPrice: 0,
        ViewsValue: 0,
        ViewsThreshold: 0,
      };
    },
    handleFileUpload(event) {
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
          this.movie.ImageBase64 = e.target.result; // Setează conținutul în Base64
        };
        reader.readAsDataURL(file); // Citește fișierul ca URL de date
      }
    },
  },
};
</script>

<style scoped>
small {
  color: #27ae60;
  font-size: 12px;
  margin-top: 5px;
  display: block;
}

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
