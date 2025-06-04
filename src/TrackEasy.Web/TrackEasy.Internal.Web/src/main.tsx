import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import './index.css'
import {createBrowserRouter, RouterProvider} from "react-router-dom";
import MainPage from "@/pages/main-page.tsx";
import Discounts from "@/pages/discounts.tsx";
import Cities from "@/pages/cities.tsx";
import Layout from "@/components/layout.tsx";
import Login from "@/pages/login.tsx";
import TwoFactor from "@/pages/two-factor.tsx";
import ProtectedRoute from "@/components/protected-route.tsx";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {Roles} from "@/lib/roles.ts";

const queryClient = new QueryClient();

const router = createBrowserRouter([
  // Public routes
  {
    path: "/login",
    element: <Login/>
  },
  {
    path: "/two-factor",
    element: <TwoFactor/>
  },
  // Protected routes
  {
    element: <ProtectedRoute><Layout/></ProtectedRoute>,
    children: [
      {
        path: "/",
        element: <MainPage/>
      },
      {
        path: "/discounts",
        element: <ProtectedRoute requiredRoles={[Roles.Admin]}><Discounts/></ProtectedRoute>
      },
      {
        path: "/cities",
        element: <ProtectedRoute requiredRoles={[Roles.Admin]}><Cities/></ProtectedRoute>
      }
    ]
  }
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router}/>
    </QueryClientProvider>
  </StrictMode>,
)
