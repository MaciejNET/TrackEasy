import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import './index.css'
import {createBrowserRouter, RouterProvider} from "react-router-dom";
import MainPage from "@/pages/main-page.tsx";
import Discounts from "@/pages/discounts.tsx";
import Cities from "@/pages/cities.tsx";
import Stations from "@/pages/stations.tsx";
import DiscountCodes from "@/pages/discount-codes.tsx";
import Operators from "@/pages/operators.tsx";
import UserManagement from "@/pages/user-management.tsx";
import Coaches from "@/pages/coaches.tsx";
import Trains from "@/pages/trains.tsx";
import Layout from "@/components/layout.tsx";
import Login from "@/pages/login.tsx";
import TwoFactor from "@/pages/two-factor.tsx";
import ConfirmEmail from "@/pages/confirm-email.tsx";
import ProtectedRoute from "@/components/protected-route.tsx";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {Roles} from "@/lib/roles.ts";
import ErrorBoundary from "@/components/error-boundary.tsx";

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
  {
    path: "/confirm",
    element: <ConfirmEmail/>
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
      },
      {
        path: "/stations",
        element: <ProtectedRoute requiredRoles={[Roles.Admin]}><Stations/></ProtectedRoute>
      },
      {
        path: "/discount-codes",
        element: <ProtectedRoute requiredRoles={[Roles.Admin]}><DiscountCodes/></ProtectedRoute>
      },
      {
        path: "/operators",
        element: <ProtectedRoute requiredRoles={[Roles.Admin]}><Operators/></ProtectedRoute>
      },
      {
        path: "/user-management",
        element: <ProtectedRoute requiredRoles={[Roles.Admin]}><UserManagement/></ProtectedRoute>
      },
      {
        path: "/coaches",
        element: <ProtectedRoute requiredRoles={[Roles.Manager]}><Coaches/></ProtectedRoute>
      },
      {
        path: "/trains",
        element: <ProtectedRoute requiredRoles={[Roles.Manager]}><Trains/></ProtectedRoute>
      }
    ]
  }
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <ErrorBoundary>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router}/>
      </QueryClientProvider>
    </ErrorBoundary>
  </StrictMode>,
)
