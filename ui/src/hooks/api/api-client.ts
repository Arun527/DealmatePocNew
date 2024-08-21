import axios from "axios";
import { getData } from "../../common/app-data";
import { environmentalVariable } from "../../common/env-variables";

const apiClient = axios.create({
  baseURL: environmentalVariable.BASE_URL,
  headers: {
    Authorization: `Bearer ${getData("token")}`,
    "Content-Type": ["application/json", "multipart/form-data", "text/plain"],
  },
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.code === "ERR_NETWORK") {
      console.log("error", error);
      return;
    } else {
      return Promise.reject(error);
    }
  }
);

export default apiClient;
