import { Button } from "@/components/ui/button";
import { ArrowLeft, Clock, MapPin, Train, Calendar } from "lucide-react";
import { useConnectionsStore } from "@/stores/connections";
import { useNavigate } from "react-router";
import { searchConnections } from "@/api/connections";
import { fetchStations } from "@/api/stations";
import { useEffect } from "react";
import { useQuery } from "@tanstack/react-query";

function Connections() {
  const navigate = useNavigate();
  const {
    searchParams,
    searchResults,
    setSearchResults,
    setSelectedJourney,
    clearSearch
  } = useConnectionsStore();

  // Debug: Log current store state
  console.log('Connections component render - Store state:', {
    searchParams,
    searchResults: !!searchResults,
    searchParamsKeys: searchParams ? Object.keys(searchParams) : null
  });

  // Fetch stations to get names
  const { data: stations = [] } = useQuery({
    queryKey: ['stations'],
    queryFn: fetchStations,
    staleTime: 5 * 60 * 1000,
    gcTime: 10 * 60 * 1000,
  });

  // Search connections using TanStack Query
  const {
    data: connectionsData,
    isLoading: isSearching,
    error: searchError,
    refetch: refetchConnections
  } = useQuery({
    queryKey: ['connections', searchParams],
    queryFn: async () => {
      if (!searchParams) {
        throw new Error('No search parameters provided');
      }

      console.log('TanStack Query: Starting search with params:', searchParams);

      // Create departure time properly without timezone issues
      const [year, month, day] = searchParams.date.split('-').map(Number);
      const [hour, minute] = searchParams.time.split(':').map(Number);

      // Create date in local timezone
      const departureDate = new Date(year, month - 1, day, hour, minute);
      const departureTime = departureDate.toISOString();

      console.log('TanStack Query: Date components:', { year, month, day, hour, minute });
      console.log('TanStack Query: Local departure date:', departureDate);
      console.log('TanStack Query: ISO departure time:', departureTime);

      const results = await searchConnections({
        startStationId: searchParams.startStationId,
        endStationId: searchParams.endStationId,
        departureTime,
      });

      console.log('TanStack Query: Search results:', results);

      // Update store with results
      setSearchResults(results);

      return results;
    },
    enabled: !!searchParams, // Only run query when we have search params
    retry: 1,
    staleTime: 0, // Always consider data stale to allow refetching
  });

  // Get station names from IDs
  const getStationName = (stationId: string) => {
    const station = stations.find(s => s.id === stationId);
    return station?.name || stationId;
  };

  // If no search params, redirect to main page
  useEffect(() => {
    console.log('Connections page mounted, searchParams:', searchParams);
    if (!searchParams) {
      console.log('No search params, redirecting to main page');
      navigate('/');
      return;
    }
  }, [searchParams, navigate]);

  const handleBackToSearch = () => {
    clearSearch();
    navigate('/');
  };

  const handleJourneySelection = (journey: any) => {
    console.log('Journey selected:', journey);
    setSelectedJourney(journey);
    navigate('/buy-ticket');
  };

  if (!searchParams) {
    return null; // Will redirect to main page
  }

  return (
    <div className="max-w-6xl mx-auto px-6 py-8">
      {/* Header */}
      <div className="flex items-center justify-between mb-8">
        <div className="flex items-center gap-4">
          <Button
            variant="outline"
            size="sm"
            onClick={handleBackToSearch}
            className="flex items-center gap-2"
          >
            <ArrowLeft className="h-4 w-4" />
            Back to Search
          </Button>
          <div>
            <h1 className="text-3xl font-bold text-gray-900">Search Results</h1>
            <p className="text-gray-600 mt-1">
              {getStationName(searchParams.startStationId)} → {getStationName(searchParams.endStationId)}
            </p>
          </div>
        </div>

        <div className="flex items-center gap-4">
          {/* Debug: Manual search button */}
          {searchParams && (
            <Button
              onClick={() => refetchConnections()}
              disabled={isSearching}
              variant="outline"
              size="sm"
              className="flex items-center gap-2"
            >
              {isSearching ? 'Searching...' : 'Refresh Search'}
            </Button>
          )}

          <div className="text-right">
            <div className="flex items-center gap-2 text-sm text-gray-600">
              <Calendar className="h-4 w-4" />
              {new Date(searchParams.date).toLocaleDateString('en-GB', {
                weekday: 'long',
                year: 'numeric',
                month: 'long',
                day: 'numeric'
              })}
            </div>
            <div className="flex items-center gap-2 text-sm text-gray-600">
              <Clock className="h-4 w-4" />
              {searchParams.time}
            </div>
          </div>
        </div>
      </div>

      {/* Loading State */}
      {isSearching && (
        <div className="text-center py-16">
          <div className="animate-spin rounded-full h-16 w-16 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-lg text-gray-600">Searching for connections...</p>
        </div>
      )}

      {/* Error State */}
      {searchError && (
        <div className="text-center py-16">
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
            <p className="font-bold">Search failed</p>
            <p>{searchError.message}</p>
            <Button
              onClick={() => refetchConnections()}
              className="mt-4"
              variant="outline"
            >
              Try Again
            </Button>
          </div>
        </div>
      )}

      {/* Search Results */}
      {connectionsData && !isSearching && (
        <div className="space-y-6">
          {connectionsData.items.length === 0 ? (
            <div className="text-center py-16">
              <Train className="h-16 w-16 text-gray-400 mx-auto mb-4" />
              <h3 className="text-xl font-semibold text-gray-900 mb-2">No connections found</h3>
              <p className="text-gray-600 mb-6">
                We couldn't find any connections for your selected criteria.
              </p>
              <Button onClick={handleBackToSearch} variant="outline">
                Modify Search
              </Button>
            </div>
          ) : (
            <>
              <div className="text-sm text-gray-600 mb-4">
                Found {connectionsData.items.length} journey option{connectionsData.items.length > 1 ? 's' : ''}
              </div>

              {connectionsData.items.map((journey, index) => (
                <div key={index} className="bg-white rounded-xl shadow-lg border border-gray-200 p-6 hover:shadow-xl transition-shadow">
                  <div className="flex justify-between items-start mb-4">
                    <div className="flex-1">
                      <h3 className="text-xl font-bold text-gray-900 mb-2">
                        {journey.startStation} → {journey.endStation}
                      </h3>
                      <div className="flex items-center gap-4 text-sm text-gray-600">
                        <div className="flex items-center gap-1">
                          <Clock className="h-4 w-4" />
                          {journey.totalDuration.substring(0, 5)}
                        </div>
                        <div className="flex items-center gap-1">
                          <MapPin className="h-4 w-4" />
                          {journey.transfersCount === 0 ? 'Direct' : `${journey.transfersCount} transfer${journey.transfersCount > 1 ? 's' : ''}`}
                        </div>
                      </div>
                    </div>

                    <div className="text-right">
                      <div className="text-2xl font-bold text-blue-600">
                        {journey.departureTime.substring(0, 5)}
                      </div>
                      <div className="text-sm text-gray-500">Departure</div>
                    </div>
                  </div>

                  <div className="border-t border-gray-100 pt-4">
                    <div className="space-y-3">
                      {journey.connections.map((connection, connIndex) => (
                        <div key={connIndex} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                          <div className="flex-1">
                            <div className="flex items-center gap-3">
                              <div className="w-3 h-3 bg-blue-500 rounded-full"></div>
                              <div>
                                <p className="font-semibold text-gray-900">{connection.name}</p>
                                <p className="text-sm text-gray-600">{connection.operatorName}</p>
                              </div>
                            </div>
                          </div>

                          <div className="text-right">
                            <div className="font-semibold text-green-600 text-lg">
                              {(() => {
                                console.log('Price data:', connection.price);
                                const currency = connection.price.currency;
                                const amount = connection.price.amount;
                                return `${currency === 'PLN' ? 'zł' : currency} ${amount.toFixed(2)}`;
                              })()}
                            </div>
                            <div className="text-sm text-gray-500">{connection.duration.substring(0, 5)}</div>
                          </div>
                        </div>
                      ))}
                    </div>
                  </div>

                  <div className="mt-4 pt-4 border-t border-gray-100">
                    <div className="flex justify-between items-center">
                      <div className="text-sm text-gray-600">
                        Arrival: {journey.arrivalTime.substring(0, 5)}
                      </div>
                      <Button
                        onClick={() => handleJourneySelection(journey)}
                        className="bg-blue-600 hover:bg-blue-700"
                      >
                        Select Journey
                      </Button>
                    </div>
                  </div>
                </div>
              ))}

              {/* Pagination */}
              {connectionsData.hasNextPage && connectionsData.nextCursor && (
                <div className="text-center mt-8">
                  <Button
                    variant="outline"
                    size="lg"
                    className="px-8"
                  // TODO: Implement pagination with nextCursor
                  >
                    Load More Results
                  </Button>
                </div>
              )}
            </>
          )}
        </div>
      )}
    </div>
  );
}

export default Connections;
