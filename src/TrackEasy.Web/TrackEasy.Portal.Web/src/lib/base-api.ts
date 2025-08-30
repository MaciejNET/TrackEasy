import axios, { AxiosResponse, AxiosRequestConfig } from "axios";

const baseApi = axios.create({
  baseURL: "https://trackeasy-api-axaaadhhapfvg8cx.polandcentral-01.azurewebsites.net",
  timeout: 10000, // 10 seconds timeout
});

// Request interceptor for common headers
baseApi.interceptors.request.use((config) => {
  // Add common headers here if needed
  // config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// Response interceptor for error handling
baseApi.interceptors.response.use(
  (response: AxiosResponse) => response,
  (error) => {
    if (error.response) {
      // Server responded with error status
      console.error('API Error:', error.response.status, error.response.data);
    } else if (error.request) {
      // Request was made but no response received
      console.error('Network Error:', error.message);
    } else {
      // Something else happened
      console.error('Request Error:', error.message);
    }
    return Promise.reject(error);
  }
);

// HTTP method wrappers
export const api = {
  get: <T = any>(url: string, config?: AxiosRequestConfig): Promise<T> => {
    console.log('Base API GET request:', url, config);
    return baseApi.get(url, config).then(response => {
      console.log('Base API GET response:', response.data);
      return response.data;
    });
  },

  post: <T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> => {
    console.log('Base API POST request:', url, data, config);
    return baseApi.post(url, data, config).then(response => {
      console.log('Base API POST response:', response.data);
      return response.data;
    });
  },

  put: <T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> => {
    console.log('Base API PUT request:', url, data, config);
    return baseApi.put(url, data, config).then(response => {
      console.log('Base API PUT response:', response.data);
      return response.data;
    });
  },

  patch: <T = any>(url: string, data?: any, config?: AxiosRequestConfig): Promise<T> => {
    console.log('Base API PATCH request:', url, data, config);
    return baseApi.patch(url, data, config).then(response => {
      console.log('Base API PATCH response:', response.data);
      return response.data;
    });
  },

  delete: <T = any>(url: string, config?: AxiosRequestConfig): Promise<T> => {
    console.log('Base API DELETE request:', url, config);
    return baseApi.delete(url, config).then(response => {
      console.log('Base API DELETE response:', response.data);
      return response.data;
    });
  },
};

export default baseApi;