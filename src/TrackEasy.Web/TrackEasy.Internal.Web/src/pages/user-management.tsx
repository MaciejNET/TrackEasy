import { useState } from "react";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { OperatorManagersTab } from "@/components/user-management/operator-managers-tab";
import { AdminsTab } from "@/components/user-management/admins-tab";

export default function UserManagement() {
  const [activeTab, setActiveTab] = useState("admins");

  return (
    <div className="space-y-4">
      <h1 className="text-2xl font-bold">User Management</h1>

      <Tabs defaultValue="admins" onValueChange={setActiveTab} value={activeTab}>
        <TabsList className="grid w-full grid-cols-2">
          <TabsTrigger value="admins">Admins</TabsTrigger>
          <TabsTrigger value="operator-managers">Operator Managers</TabsTrigger>
        </TabsList>
        <TabsContent value="admins">
          <AdminsTab />
        </TabsContent>
        <TabsContent value="operator-managers">
          <OperatorManagersTab />
        </TabsContent>
      </Tabs>
    </div>
  );
}
