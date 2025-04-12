import {toast} from "sonner";

export const BASE_URL = "http://localhost:5222";

async function handleResponse<T>(res: Response): Promise<T> {
  if (!res.ok) {
    let errorMessage = res.statusText;
    try {
      const errorData = await res.json();
      errorMessage = errorData.detail || res.statusText;
      toast.error(errorMessage);
    } catch (err) {
      console.error('Error parsing error response:', err);
    }
    throw new Error(errorMessage);
  }
  
  const text = await res.text();
  return text ? JSON.parse(text) as T : ({} as T);
}

export const baseAPI = {
  get: async <T>(url: string, params?: URLSearchParams): Promise<T> => {
    let fullUrl = url;
    if (params && params.toString()) {
      fullUrl += '?' + params.toString();
    }
    const res = await fetch(fullUrl);
    return handleResponse<T>(res);
  },

  post: async <T>(url: string, body: unknown): Promise<T> => {
    const res = await fetch(url, {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(body)
    });
    return handleResponse<T>(res);
  },

  put: async <T>(url: string, body: unknown): Promise<T> => {
    const res = await fetch(url, {
      method: 'PUT',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(body)
    });
    return handleResponse<T>(res);
  },

  patch: async <T>(url: string, body: unknown): Promise<T> => {
    const res = await fetch(url, {
      method: 'PATCH',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(body)
    });
    return handleResponse<T>(res);
  },

  delete: async <T>(url: string): Promise<T> => {
    const res = await fetch(url, {method: 'DELETE'});
    return handleResponse<T>(res);
  }
};