import { create } from "zustand/react";
import { 
  fetchNotifications, 
  fetchNotificationsCount, 
  markAsRead as markNotificationAsRead,
  NotificationDto 
} from "@/api/notifications-api.ts";
import { PaginatedResult } from "@/types/paginated-result.ts";
import { notificationHub } from "@/lib/notification-hub.ts";

interface NotificationsState {
  notifications: PaginatedResult<NotificationDto> | null;
  count: number;
  isLoading: boolean;
  isCountLoading: boolean;
  error: string | null;

  
  fetchNotifications: (pageNumber: number, pageSize: number) => Promise<void>;
  fetchCount: () => Promise<void>;
  markAsRead: (id: string) => Promise<void>;
  initializeHub: () => Promise<void>;
}

export const useNotificationsStore = create<NotificationsState>((set, get) => ({
  notifications: null,
  count: 0,
  isLoading: false,
  isCountLoading: false,
  error: null,

  fetchNotifications: async (pageNumber: number, pageSize: number) => {
    set({ isLoading: true, error: null });

    try {
      const data = await fetchNotifications({ pageNumber, pageSize });
      set({
        notifications: data,
        isLoading: false
      });
    } catch (error) {
      set({
        isLoading: false,
        error: error instanceof Error ? error.message : "Failed to fetch notifications"
      });
    }
  },

  fetchCount: async () => {
    set({ isCountLoading: true });

    try {
      const count = await fetchNotificationsCount();
      set({
        count,
        isCountLoading: false
      });
    } catch (error) {
      set({
        isCountLoading: false,
        error: error instanceof Error ? error.message : "Failed to fetch notification count"
      });
      console.error("Error fetching notification count:", error);
    }
  },

  markAsRead: async (id: string) => {
    try {
      
      await markNotificationAsRead(id);

      
      const { count } = get();
      if (count > 0) {
        set({ count: count - 1 });
      }

      
      const { notifications } = get();
      if (notifications) {
        await get().fetchNotifications(notifications.pageNumber, notifications.pageSize);
      }
    } catch (error) {
      console.error("Error marking notification as read:", error);
      set({
        error: error instanceof Error ? error.message : "Failed to mark notification as read"
      });
    }
  },

  initializeHub: async () => {
    
    await notificationHub.start();

    
    notificationHub.onNotification(() => {
      
      get().fetchCount();
    });

    
    await get().fetchCount();
  }
}));
