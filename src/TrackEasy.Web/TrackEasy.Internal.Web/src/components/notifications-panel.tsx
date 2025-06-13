import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Bell } from "lucide-react";
import { format } from "date-fns";
import { 
  Popover, 
  PopoverContent, 
  PopoverTrigger 
} from "@/components/ui/popover.tsx";
import { Button } from "@/components/ui/button.tsx";
import { Separator } from "@/components/ui/separator.tsx";
import { useNotificationsStore } from "@/stores/notifications-store.ts";
import { useUserStore } from "@/stores/user-store.ts";
import { NotificationType } from "@/api/notifications-api.ts";
import { cn } from "@/lib/utils";

export function NotificationsPanel() {
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);
  const { 
    notifications, 
    count, 
    isLoading, 
    fetchNotifications, 
    markAsRead,
    initializeHub 
  } = useNotificationsStore();
  const { user } = useUserStore();

  // Initialize the notification hub and fetch notifications when the component mounts
  useEffect(() => {
    initializeHub();
  }, [initializeHub]);

  // Fetch notifications when the popover is opened
  useEffect(() => {
    if (open) {
      fetchNotifications(1, 10); // Fetch first page with 10 items
    }
  }, [open, fetchNotifications]);

  const handleNotificationClick = (type: NotificationType, objectId: string) => {
    // Close the popover
    setOpen(false);

    // Navigate to the appropriate page based on the notification type
    switch (type) {
      case NotificationType.REFUND_REQUEST:
        navigate(`/refund-requests?id=${objectId}`);
        break;
      case NotificationType.CONNECTION_REQUEST:
        // Different navigation based on user role
        if (user) {
          if (user.role === 'Admin') {
            navigate(`/connection-requests?id=${objectId}`);
          } else if (user.role === 'OperatorManager') {
            navigate(`/connections?id=${objectId}`);
          } else {
            // Default fallback for other roles
            navigate(`/connection-requests?id=${objectId}`);
          }
        } else {
          // Fallback if user is not loaded yet
          navigate(`/connection-requests?id=${objectId}`);
        }
        break;
      default:
        console.warn(`Unknown notification type: ${type}`);
    }
  };

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button variant="outline" size="icon" className="relative">
          <Bell className="h-5 w-5" />
          {count > 0 && (
            <span className="absolute -top-1 -right-1 flex h-5 w-5 items-center justify-center rounded-full bg-destructive text-xs text-white">
              {count > 99 ? "99+" : count}
            </span>
          )}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-80 p-0" align="end">
        <div className="p-4 font-medium">Notifications</div>
        <Separator />
        <div className="max-h-80 overflow-y-auto">
          {isLoading ? (
            <div className="flex justify-center items-center p-4">
              <span>Loading...</span>
            </div>
          ) : notifications?.items.length === 0 ? (
            <div className="p-4 text-center text-muted-foreground">
              No notifications
            </div>
          ) : (
            <div>
              {notifications?.items.map((notification) => (
                <div
                  key={notification.id}
                  className="p-4 border-b last:border-b-0 hover:bg-muted cursor-pointer"
                  onClick={() => {
                    handleNotificationClick(notification.type, notification.objectId);
                  }}
                >
                  <div className="flex justify-between items-start mb-1">
                    <h4 className="font-medium">{notification.title}</h4>
                    <span className="text-xs text-muted-foreground">
                      {format(new Date(notification.createdAt), "MMM d, h:mm a")}
                    </span>
                  </div>
                  <p className="text-sm text-muted-foreground mb-2">
                    {notification.message}
                  </p>
                  <Button 
                    variant="outline" 
                    size="sm" 
                    className="w-full"
                    onClick={(e) => {
                      e.stopPropagation(); // Prevent parent onClick from firing
                      markAsRead(notification.id);
                    }}
                  >
                    Mark as Read
                  </Button>
                </div>
              ))}
            </div>
          )}
        </div>
        {notifications && notifications.totalPages > 1 && (
          <div className="p-2 border-t flex justify-center">
            <Button 
              variant="ghost" 
              size="sm"
              onClick={() => fetchNotifications(1, 10)}
              disabled={notifications.pageNumber === 1}
            >
              View All
            </Button>
          </div>
        )}
      </PopoverContent>
    </Popover>
  );
}
