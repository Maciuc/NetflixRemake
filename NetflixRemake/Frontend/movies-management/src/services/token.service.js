let token = {
    getJwtToken: function () {
      const token = localStorage.getItem("jwtToken")
      if (token != 'undefined') return JSON.parse(localStorage.getItem("jwtToken"));
    },

    getRefreshToken: function () {
      if (localStorage.getItem("refreshToken"))
        return JSON.parse(localStorage.getItem("refreshToken"));
    },
  
    setJwtToken: function (token) {
      localStorage.setItem("jwtToken", JSON.stringify(token));
    },
    setRefreshToken: function (token) {
      localStorage.setItem("refreshToken", JSON.stringify(token));
    },

    removeAuthTokens: function (token) {
      localStorage.removeItem("jwtToken");
      localStorage.removeItem("refreshToken");
      localStorage.removeItem("jwt");
    },
  };
  
  export default token;