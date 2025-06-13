import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {useForm} from "react-hook-form";
import {useTrainStore} from "@/stores/train-store.ts";
import {zodResolver} from "@hookform/resolvers/zod";
import {TrainSearchData, trainSearchSchema} from "@/schemas/train-search-schema.ts";

type TrainSearchFormProps = {
  onAdd: () => void,
};

export function TrainSearchForm(props: TrainSearchFormProps) {
  const {setSearchParams} = useTrainStore();
  const {onAdd} = props;

  const {
    register,
    handleSubmit,
    formState: {errors},
  } = useForm<TrainSearchData>({
    mode: "onChange",
    resolver: zodResolver(trainSearchSchema),
  });

  const onSubmit = (values: TrainSearchData) => {
    setSearchParams({
      trainName: values.trainName || '',
      pageNumber: 1
    });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="w-full p-4 space-y-4">
      <div className="grid grid-cols-12 items-center gap-4">
        <label htmlFor="trainName" className="col-span-1 col-start-1 font-medium">
          Trains
        </label>
        <div className="col-span-4">
          <Input
            id="trainName"
            placeholder="Train Name"
            {...register("trainName")}
          />
          {errors.trainName && (
            <p className="text-red-500 text-sm">{errors.trainName.message}</p>
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