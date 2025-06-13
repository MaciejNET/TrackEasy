import {Input} from "@/components/ui/input.tsx";
import {Button} from "@/components/ui/button.tsx";
import {useForm} from "react-hook-form";
import {useStationStore} from "@/stores/station-store.ts";
import {zodResolver} from "@hookform/resolvers/zod";
import {StationSearchData, stationSearchSchema} from "@/schemas/station-search-schema.ts";

type StationSearchFormProps = {
  onAdd: () => void,
};

export function StationSearchForm(props: StationSearchFormProps) {
  const {setSearchParams} = useStationStore();
  const {onAdd} = props;

  const {
    register,
    handleSubmit,
    formState: {errors},
  } = useForm<StationSearchData>({
    mode: "onChange",
    resolver: zodResolver(stationSearchSchema),
  });

  const onSubmit = (values: StationSearchData) => {
    setSearchParams({
      stationName: values.stationName,
      cityName: values.cityName,
      pageNumber: 1
    });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="w-full p-4 space-y-4">
      <div className="grid grid-cols-12 items-center gap-4">
        <label htmlFor="stationName" className="col-span-1 col-start-1 font-medium">
          Stations
        </label>
        <div className="col-span-4">
          <Input
            id="stationName"
            placeholder="Station Name"
            {...register("stationName")}
          />
          {errors.stationName && (
            <p className="text-red-500 text-sm">{errors.stationName.message}</p>
          )}
        </div>
        <div className="col-span-4">
          <Input
            id="cityName"
            placeholder="City Name"
            {...register("cityName")}
          />
          {errors.cityName && (
            <p className="text-red-500 text-sm">{errors.cityName.message}</p>
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