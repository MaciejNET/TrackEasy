import {toast} from "sonner";
import {getToken} from "@/lib/auth-storage.ts";

export const BASE_URL = "http://localhost:5222";

function getHeaders(contentType = true): HeadersInit {
  const headers: HeadersInit = {};

  if (contentType) {
    headers['Content-Type'] = 'application/json';
  }

  const token = getToken();
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  return headers;
}

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

  if (!text) {
    return ({} as T);
  }

  if (!text.startsWith('{') && !text.startsWith('[')) {
    return text as unknown as T;
  }

  try {
    return JSON.parse(text) as T;
  } catch (err) {
    console.error('Error parsing JSON response:', err);
    return ({} as T);
  }
}

export const baseAPI = {
  get: async <T>(url: string, params?: URLSearchParams): Promise<T> => {
    let fullUrl = url;
    if (params && params.toString()) {
      fullUrl += '?' + params.toString();
    }
    const res = await fetch(fullUrl, {
      headers: getHeaders(false)
    });
    return handleResponse<T>(res);
  },

  post: async <T>(url: string, body: unknown): Promise<T> => {
    const res = await fetch(url, {
      method: 'POST',
      headers: getHeaders(),
      body: JSON.stringify(body)
    });
    return handleResponse<T>(res);
  },

  put: async <T>(url: string, body: unknown): Promise<T> => {
    const res = await fetch(url, {
      method: 'PUT',
      headers: getHeaders(),
      body: JSON.stringify(body)
    });
    return handleResponse<T>(res);
  },

  patch: async <T>(url: string, body: unknown): Promise<T> => {
    const res = await fetch(url, {
      method: 'PATCH',
      headers: getHeaders(),
      body: JSON.stringify(body)
    });
    return handleResponse<T>(res);
  },

  delete: async <T>(url: string): Promise<T> => {
    const res = await fetch(url, {
      method: 'DELETE',
      headers: getHeaders(false)
    });
    return handleResponse<T>(res);
  }
};
