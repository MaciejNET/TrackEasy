import { createBrowserRouter, RouterProvider, Outlet } from 'react-router';
import { Layout } from './components/layout';
import Main from './pages/main';
import Connections from './pages/connections';
import BuyTicket from './pages/buy-ticket';

const router = createBrowserRouter([
  {
    path: '/',
    element: (
      <Layout>
        <Outlet />
      </Layout>
    ),
    children: [
      {
        index: true,
        element: <Main />,
      },
      {
        path: 'connections',
        element: <Connections />,
      },
      {
        path: 'buy-ticket',
        element: <BuyTicket />,
      },
    ],
  },
]);

export function App() {
  return <RouterProvider router={router} />;
}
