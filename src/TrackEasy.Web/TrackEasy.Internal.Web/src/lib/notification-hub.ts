import * as signalR from "@microsoft/signalr";
import {getToken} from "@/lib/auth-storage.ts";
import {BASE_URL} from "@/lib/api-constants.ts";

// Define the callback type for notification events
type NotificationCallback = () => void;

class NotificationHubConnection {
  private connection: signalR.HubConnection | null = null;
  private callbacks: NotificationCallback[] = [];
  private isConnecting = false;

  // Register a callback to be called when a notification is received
  public onNotification(callback: NotificationCallback): () => void {
    this.callbacks.push(callback);

    // Return a function to unregister the callback
    return () => {
      this.callbacks = this.callbacks.filter(cb => cb !== callback);
    };
  }

  // Start the connection to the notification hub
  public async start(): Promise<void> {
    if (this.connection || this.isConnecting) {
      return;
    }

    this.isConnecting = true;

    try {
      const token = getToken();
      if (!token) {
        console.error("No authentication token available");
        this.isConnecting = false;
        return;
      }

      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(`${BASE_URL}/hubs/notification`, {
          accessTokenFactory: () => token,
        })
        .withAutomaticReconnect()
        .build();

      // Register for the "ReceiveNotification" event
      this.connection.on("ReceiveNotification", () => {
        this.notifyCallbacks();
      });

      // Handle connection closed event
      this.connection.onclose(() => {
        console.log("SignalR connection closed");
      });

      await this.connection.start();
      console.log("SignalR connection established");

      // Notify callbacks on initial connection
      this.notifyCallbacks();
    } catch (error) {
      console.error("Error establishing SignalR connection:", error);
      this.connection = null;
    } finally {
      this.isConnecting = false;
    }
  }

  // Stop the connection to the notification hub
  public async stop(): Promise<void> {
    if (this.connection) {
      try {
        await this.connection.stop();
        console.log("SignalR connection stopped");
      } catch (error) {
        console.error("Error stopping SignalR connection:", error);
      } finally {
        this.connection = null;
      }
    }
  }

  // Notify all registered callbacks
  private notifyCallbacks(): void {
    this.callbacks.forEach(callback => {
      try {
        callback();
      } catch (error) {
        console.error("Error in notification callback:", error);
      }
    });
  }
}

// Create a singleton instance
export const notificationHub = new NotificationHubConnection();
