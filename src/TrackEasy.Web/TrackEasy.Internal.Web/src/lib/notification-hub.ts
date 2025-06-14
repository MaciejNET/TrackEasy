import * as signalR from "@microsoft/signalr";
import {getToken} from "@/lib/auth-storage.ts";
import {BASE_URL} from "@/lib/api-constants.ts";
import {useAuthStore} from "@/stores/auth-store.ts";


type NotificationCallback = () => void;

class NotificationHubConnection {
  private connection: signalR.HubConnection | null = null;
  private callbacks: NotificationCallback[] = [];
  private isConnecting = false;

  
  public onNotification(callback: NotificationCallback): () => void {
    this.callbacks.push(callback);

    
    return () => {
      this.callbacks = this.callbacks.filter(cb => cb !== callback);
    };
  }

  
  public async start(): Promise<void> {
    if (this.connection || this.isConnecting) {
      return;
    }

    this.isConnecting = true;

    try {
      
      if (!useAuthStore.getState().checkAuth()) {
        console.error("User is not authenticated. Cannot connect to notification hub.");
        this.isConnecting = false;
        return;
      }

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

      
      this.connection.on("ReceiveNotification", () => {
        this.notifyCallbacks();
      });

      
      this.connection.onclose(() => {
        console.log("SignalR connection closed");
      });

      await this.connection.start();
      console.log("SignalR connection established");

      
      this.notifyCallbacks();
    } catch (error) {
      console.error("Error establishing SignalR connection:", error);
      this.connection = null;
    } finally {
      this.isConnecting = false;
    }
  }

  
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


export const notificationHub = new NotificationHubConnection();
