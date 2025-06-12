import {Card, CardContent, CardDescription, CardHeader, CardTitle} from "@/components/ui/card.tsx";
import {useConnectionStore} from "@/stores/connection-store.ts";
import {Label} from "@/components/ui/label.tsx";
import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {PlusIcon} from "lucide-react";
import {useEffect} from "react";
import {Form, FormControl, FormField, FormItem} from "@/components/ui/form.tsx";
import {useForm} from "react-hook-form";
import {zodResolver} from "@hookform/resolvers/zod";
import {z} from "zod";
import {useUserStore} from "@/stores/user-store.ts";

const searchFormSchema = z.object({
  name: z.string().optional(),
  startStation: z.string().optional(),
  endStation: z.string().optional(),
});

type SearchFormValues = z.infer<typeof searchFormSchema>;

type ConnectionSearchFormProps = {
  onAdd: () => void;
};

export function ConnectionSearchForm(props: ConnectionSearchFormProps) {
  const {onAdd} = props;
  const {searchParams, setSearchParams, resetSearchParams} = useConnectionStore();
  const {user} = useUserStore();

  const form = useForm<SearchFormValues>({
    resolver: zodResolver(searchFormSchema),
    defaultValues: {
      name: searchParams.name || "",
      startStation: searchParams.startStation || "",
      endStation: searchParams.endStation || "",
    },
  });

  // Set the operator ID from the user store when the component mounts
  useEffect(() => {
    if (user?.operatorId) {
      setSearchParams({operatorId: user.operatorId, pageNumber: 1});
    }
  }, [user, setSearchParams]);

  const onSubmit = (values: SearchFormValues) => {
    if (user?.operatorId) {
      setSearchParams({
        ...values,
        operatorId: user.operatorId,
        pageNumber: 1,
      });
    }
  };

  const handleReset = () => {
    form.reset({
      name: "",
      startStation: "",
      endStation: "",
    });
    if (user?.operatorId) {
      resetSearchParams(user.operatorId);
    }
  };

  return (
    <Card className="mb-6">
      <CardHeader className="flex flex-row items-center justify-between">
        <div>
          <CardTitle>Connections</CardTitle>
          <CardDescription>
            Manage your train connections
          </CardDescription>
        </div>
        <Button onClick={onAdd} disabled={!user?.operatorId}>
          <PlusIcon className="h-4 w-4 mr-2" />
          Add Connection
        </Button>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">

            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <FormField
                control={form.control}
                name="name"
                render={({ field }) => (
                  <FormItem>
                    <Label htmlFor="name">Connection Name</Label>
                    <FormControl>
                      <Input id="name" placeholder="Search by name" {...field} />
                    </FormControl>
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="startStation"
                render={({ field }) => (
                  <FormItem>
                    <Label htmlFor="startStation">Start Station</Label>
                    <FormControl>
                      <Input id="startStation" placeholder="Search by start station" {...field} />
                    </FormControl>
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="endStation"
                render={({ field }) => (
                  <FormItem>
                    <Label htmlFor="endStation">End Station</Label>
                    <FormControl>
                      <Input id="endStation" placeholder="Search by end station" {...field} />
                    </FormControl>
                  </FormItem>
                )}
              />
            </div>

            <div className="flex justify-end gap-2">
              <Button
                type="button"
                variant="outline"
                onClick={handleReset}
                disabled={!user?.operatorId}
              >
                Reset
              </Button>
              <Button
                type="submit"
                disabled={!user?.operatorId}
              >
                Search
              </Button>
            </div>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
