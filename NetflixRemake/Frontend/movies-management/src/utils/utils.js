export default {
  //#region DateFormat

  FormatDateYearMonthDay(date) {
    return moment(date).format("YYYY-MM-DD");
  },
  FormatDateDayMonthYear(date) {
    return moment(date).format("DD/MM/YYYY");
  },
  FormatDateDayMonthYearPoint(date) {
    return date ? moment(date).format("DD.MM.YYYY") : "-";
  },

  FormatDateMonthYearPoint(date) {
    return date ? moment(date).format("MM.YYYY") : "-";
  },

  FormatDateDayTextMonthYear(date) {
    return moment(date).format("DD MMMM YYYY");
  },

  FormatDatePoint(date) {
    // return moment(date, 'DD.MM.YYYY').format('DD.MM.YYYY');
    return moment(date).format("DD.MM.YYYY");
  },

  //#endregion
  //#region Toasts
  ToastNotify(Type, Title, Timer) {
    if (!Timer) Timer = 2500;
    Swal.fire({
      toast: true,
      position: "top-right",
      showConfirmButton: false,
      timer: Timer,
      timerProgressBar: true,
      icon: Type,
      title: Title,
    });
  },
  ToastNotifySomethingWentWrong(Timer) {
    if (!Timer) Timer = 2500;
    Swal.fire({
      toast: true,
      position: "top-right",
      showConfirmButton: false,
      timer: Timer,
      timerProgressBar: true,
      icon: "error",
      title: "Something went wrong!",
    });
  },

  //#endregion
  //#region Clipboard
  CopyText(text) {
    navigator.clipboard.writeText(text);
    //   .then(() => {
    //     // this.copySuccess = true;
    //   //   setTimeout(() => {
    //   //     this.copySuccess = false;
    //   //   }, 2000); // Message disappears after 2 seconds
    //   // }).catch(err => {
    //   //   console.error('Failed to copy text: ', err);
    //   // });
    // },
  },
  //#endregion
  //#region DynamicImage

  ShowDynamicImagePng(imagePath, defaultImg) {
    if (!imagePath) {
      const defaultimg = `images/${defaultImg}.png`;
      return new URL(`/src/assets/${defaultimg}`, import.meta.url).href;
      // return `/src/assets/images/${defaultImg}.png`;
    }
    return imagePath;
  },

  ShowDynamicImageIconItem(imagePath, defaultImg) {
    if (!imagePath) {
      const defaultimg = `images/${defaultImg}.png`;
      return new URL(`/src/assets/${defaultimg}`, import.meta.url).href;
      // return `/src/assets/images/${defaultImg}.png`;
    }
    return imagePath;
  },

  ShowDynamicImage(imagePath, defaultImg) {
    if (!imagePath) {
      const defaultimg = `images/${defaultImg}.png`;
      return new URL(`/src/assets/${defaultimg}`, import.meta.url).href;
      // return `/src/assets/images/${defaultImg}.png`;
    }
    return imagePath;
  },

  ShowDynamicImageSvg(imagePath, defaultImg) {
    if (!imagePath) {
      const defaultimg = `images/${defaultImg}.svg`;
      return new URL(`/src/assets/${defaultimg}`, import.meta.url).href;
      // return `/src/assets/images/${defaultImg}.png`;
    }
    return imagePath;
  },

  ShowImageUserNotFoundOnServer(event) {
    event.target.onerror = null;
    event.target.src = new URL(
      `/src/assets/images/default-img.png`,
      import.meta.url
    ).href;
  },

  ShowImageNotFoundOnServer(event) {
    event.target.onerror = null;
    event.target.src = new URL(
      `/src/assets/images/default-img.png`,
      import.meta.url
    ).href;
  },
  //#endregion
  //#region TextFormatter

  FormatNumber(number) {
    return new Intl.NumberFormat("de-DE").format(number);
  },

  TextLimiter(text, charactersLimit, defaultString) {
    if (text) {
      return text.length > charactersLimit
        ? text.substring(0, charactersLimit) + "..."
        : text;
    } else {
      return defaultString;
    }
  },

  WordsLimit(text, wordLimit) {
    const words = text.split(" ");
    const limitedWords = words.slice(0, wordLimit);
    return limitedWords.join(" ") + (words.length > wordLimit ? "..." : "");
  },

  //#endregion
  //#region InputImage
  InputImage(item, property) {
    const fileInput = event.target;
    const file = fileInput.files[0];
    if (file && !file.type.includes("image")) {
      this.ToastNotify("warning", "Only images are allowed.");
      return;
    }
    // 2 544 856 == 2.42mb
    if (file && file.size > 1055000) {
      this.ToastNotify("warning", "Image size should be maximum 1MB.");
      return;
    }
    const reader = new FileReader();
    reader.onload = () => {
      item[property] = reader.result;
      fileInput.value = "";
    };
    if (file) {
      reader.readAsDataURL(file);
    }
  },

  DeletePhoto(item, warningUploadImage) {
    item.ImageBase64 = "";
    warningUploadImage.text = "";
  },

  StringToSlug(text) {
    return text
      .toLowerCase()
      .trim()
      .normalize("NFD")
      .replace(/[^\w\s]/g, "")
      .replace(/\s+/g, "-")
      .replace(/-+/g, "-");
  },
};
//#endregion
