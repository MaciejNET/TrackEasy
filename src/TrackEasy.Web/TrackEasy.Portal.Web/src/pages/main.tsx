import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Search, Train, MapPin, Calendar, Clock } from "lucide-react";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { useNavigate } from "react-router";
import { fetchStations } from "@/api/stations";
import { useConnectionsStore } from "@/stores/connections";

function Main() {
  const navigate = useNavigate();
  const { setSearchParams, setIsSearching } = useConnectionsStore();

  // Form state
  const [startStationId, setStartStationId] = useState<string>("");
  const [endStationId, setEndStationId] = useState<string>("");
  const [date, setDate] = useState<string>("");
  const [time, setTime] = useState<string>("");

  // Fetch stations
  const { data: stations = [], isLoading, error } = useQuery({
    queryKey: ['stations'],
    queryFn: fetchStations,
    staleTime: 5 * 60 * 1000,
    gcTime: 10 * 60 * 1000,
  });

  // Form validation
  const isFormValid = startStationId && endStationId && date && time;

  // Handle search
  const handleSearch = async () => {
    if (!isFormValid) return;

    console.log('Search button clicked with form data:', {
      startStationId,
      endStationId,
      date,
      time
    });

    try {
      setIsSearching(true);

      // Store search params in Zustand store
      const searchParams = {
        startStationId,
        endStationId,
        date,
        time,
      };

      console.log('Setting search params in store:', searchParams);
      setSearchParams(searchParams);

      // Navigate to connections page
      navigate('/connections');
    } catch (error) {
      console.error('Navigation failed:', error);
      setIsSearching(false);
    }
  };

  if (isLoading) {
    return (
      <div className="max-w-6xl mx-auto px-6 py-16">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Loading stations...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="max-w-6xl mx-auto px-6 py-16">
        <div className="text-center">
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            <p className="font-bold">Error loading stations</p>
            <p>{error.message}</p>
          </div>
        </div>
      </div>
    );
  }

  return (
    <>
      {/* Main Content */}
      <div className="max-w-6xl mx-auto px-6 py-16">
        {/* Hero Section */}
        <div className="text-center mb-12">
          <h2 className="text-4xl font-bold text-gray-900 mb-4">
            Find Your Perfect Train Journey
          </h2>
          <p className="text-lg text-gray-600 max-w-2xl mx-auto">
            Search for train connections across the network. Get real-time schedules,
            platform information, and plan your journey with ease.
          </p>
        </div>

        {/* Search Form */}
        <div className="bg-white rounded-2xl shadow-xl border border-gray-200/50 p-8">
          <div className="grid grid-cols-4 gap-2 mb-8">
            {/* Departure Station */}
            <div className="space-y-2">
              <label className="text-sm font-medium text-gray-700 flex items-center gap-2">
                <MapPin className="h-4 w-4 text-blue-600" />
                From
              </label>
              <Select value={startStationId} onValueChange={setStartStationId}>
                <SelectTrigger className="!h-9 w-full border-gray-200 hover:border-blue-300 focus:border-blue-500 transition-colors">
                  <SelectValue placeholder="Select departure station" />
                </SelectTrigger>
                <SelectContent>
                  {stations.map((station) => (
                    <SelectItem key={station.id} value={station.id}>
                      {station.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            {/* Arrival Station */}
            <div className="space-y-2">
              <label className="text-sm font-medium text-gray-700 flex items-center gap-2">
                <MapPin className="h-4 w-4 text-green-600" />
                To
              </label>
              <Select value={endStationId} onValueChange={setEndStationId}>
                <SelectTrigger className="!h-9 w-full border-gray-200 hover:border-blue-300 focus:border-blue-500 transition-colors">
                  <SelectValue placeholder="Select arrival station" />
                </SelectTrigger>
                <SelectContent>
                  {stations.map((station) => (
                    <SelectItem key={station.id} value={station.id}>
                      {station.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            {/* Date */}
            <div className="space-y-2">
              <label className="text-sm font-medium text-gray-700 flex items-center gap-2">
                <Calendar className="h-4 w-4 text-purple-600" />
                Date
              </label>
              <input
                type="date"
                value={date}
                onChange={(e) => setDate(e.target.value)}
                className="w-full h-9 px-3 border border-gray-200 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors text-gray-900"
                min={new Date().toISOString().split('T')[0]}
              />
            </div>

            {/* Time */}
            <div className="space-y-2">
              <label className="text-sm font-medium text-gray-700 flex items-center gap-2">
                <Clock className="h-4 w-4 text-orange-600" />
                Time
              </label>
              <input
                type="time"
                value={time}
                onChange={(e) => setTime(e.target.value)}
                className="w-full h-9 px-3 border border-gray-200 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors text-gray-900"
              />
            </div>
          </div>

          {/* Search Button */}
          <div className="flex justify-center">
            <Button
              onClick={handleSearch}
              disabled={!isFormValid}
              className={`h-10 px-8 text-base font-semibold shadow-lg hover:shadow-xl transition-all duration-200 transform hover:scale-105 ${isFormValid
                ? 'bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700'
                : 'bg-gray-400 cursor-not-allowed'
                }`}
            >
              <Search className="h-4 w-4 mr-2" />
              Search Connections
            </Button>
          </div>
        </div>

        {/* Quick Actions */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mt-12">
          <div className="bg-white/60 backdrop-blur-sm rounded-xl p-6 border border-gray-200/50 hover:shadow-lg transition-all duration-200">
            <div className="flex items-center gap-3 mb-3">
              <div className="p-2 bg-blue-100 rounded-lg">
                <Clock className="h-5 w-5 text-blue-600" />
              </div>
              <h3 className="font-semibold text-gray-900">Live Updates</h3>
            </div>
            <p className="text-gray-600 text-sm">Get real-time platform changes and delays</p>
          </div>

          <div className="bg-white/60 backdrop-blur-sm rounded-xl p-6 border border-gray-200/50 hover:shadow-lg transition-all duration-200">
            <div className="flex items-center gap-3 mb-3">
              <div className="p-2 bg-green-100 rounded-lg">
                <MapPin className="h-5 w-5 text-green-600" />
              </div>
              <h3 className="font-semibold text-gray-900">Platform Info</h3>
            </div>
            <p className="text-gray-600 text-sm">Find your platform and departure details</p>
          </div>

          <div className="bg-white/60 backdrop-blur-sm rounded-xl p-6 border border-gray-200/50 hover:shadow-lg transition-all duration-200">
            <div className="flex items-center gap-3 mb-3">
              <div className="p-2 bg-purple-100 rounded-lg">
                <Train className="h-5 w-5 text-purple-600" />
              </div>
              <h3 className="font-semibold text-gray-900">Journey Planning</h3>
            </div>
            <p className="text-gray-600 text-sm">Plan multi-leg journeys with ease</p>
          </div>
        </div>
      </div>
    </>
  );
}

export default Main;