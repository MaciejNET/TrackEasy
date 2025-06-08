import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {useForm} from "react-hook-form";
import {useCoachStore} from "@/stores/coach-store.ts";
import {zodResolver} from "@hookform/resolvers/zod";
import {CoachSearchData, coachSearchSchema} from "@/schemas/coach-search-schema.ts";

type CoachSearchFormProps = {
  onAdd: () => void,
};

export function CoachSearchForm(props: CoachSearchFormProps) {
  const {setSearchParams} = useCoachStore();
  const {onAdd} = props;

  const {
    register,
    handleSubmit,
    formState: {errors},
  } = useForm<CoachSearchData>({
    mode: "onChange",
    resolver: zodResolver(coachSearchSchema),
  });

  const onSubmit = (values: CoachSearchData) => {
    setSearchParams({
      code: values.code || '',
      pageNumber: 1
    });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="w-full p-4 space-y-4">
      <div className="grid grid-cols-12 items-center gap-4">
        <label htmlFor="code" className="col-span-1 col-start-1 font-medium">
          Coaches
        </label>
        <div className="col-span-4">
          <Input
            id="code"
            placeholder="Code"
            {...register("code")}
          />
          {errors.code && (
            <p className="text-red-500 text-sm">{errors.code.message}</p>
          )}
        </div>
        <div className="col-span-2 col-start-11 space-y-2 flex flex-col">
          <Button type="submit">Search</Button>
          <Button type="button" variant="outline" onClick={onAdd}>
            Add
          </Button>
        </div>
      </div>
    </form>
  );
}