import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ArrowLeft, Plus, Trash2, User, Check, X } from "lucide-react";
import { useConnectionsStore } from "@/stores/connections";
import { useNavigate } from "react-router";
import { getTicketPrice } from "@/api/tickets";
import { fetchDiscounts, validateDiscountCode } from "@/api/discounts";
import { Passenger, TicketPriceResponse, Discount, DiscountCode } from "@/lib/schemas";
import { useEffect, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";

function BuyTicket() {
  const navigate = useNavigate();
  const { selectedJourney, searchParams, clearSearch } = useConnectionsStore();

  // Local state for passengers and discount
  const [passengers, setPassengers] = useState<Passenger[]>([
    { firstName: '', lastName: '', dateOfBirth: '', discountId: null }
  ]);
  const [discountCodeId, setDiscountCodeId] = useState<string | null>(null);
  const [discountCodeInput, setDiscountCodeInput] = useState<string>('');
  const [discountCode, setDiscountCode] = useState<DiscountCode | null>(null);
  const [isValidatingCode, setIsValidatingCode] = useState(false);
  const [ticketPrice, setTicketPrice] = useState<TicketPriceResponse | null>(null);

  // Fetch available discounts
  const { data: discounts = [], isLoading: isDiscountsLoading } = useQuery({
    queryKey: ['discounts'],
    queryFn: fetchDiscounts,
    staleTime: 5 * 60 * 1000, // 5 minutes
    gcTime: 10 * 60 * 1000, // 10 minutes
  });

  // If no selected journey, redirect to main page
  useEffect(() => {
    if (!selectedJourney || !searchParams) {
      console.log('No selected journey or search params, redirecting to main page');
      navigate('/');
      return;
    }
  }, [selectedJourney, searchParams, navigate]);

  // Get ticket price when passengers or discount changes
  const {
    data: priceData,
    isLoading: isPriceLoading,
    error: priceError,
    refetch: refetchPrice
  } = useQuery({
    queryKey: ['ticketPrice', passengers, discountCodeId, selectedJourney],
    queryFn: async () => {
      if (!selectedJourney || !searchParams) {
        throw new Error('No journey or search params available');
      }

      const request = {
        passengers: passengers.filter(p => p.firstName && p.lastName && p.dateOfBirth),
        discountCodeId,
        connections: selectedJourney.connections.map(conn => ({
          id: conn.id,
          startStationId: conn.departureStationId,
          endStationId: conn.arrivalStationId,
          connectionDate: searchParams.date,
        })),
      };

      console.log('Requesting ticket price:', request);
      const result = await getTicketPrice(request);
      setTicketPrice(result);
      return result;
    },
    enabled: !!selectedJourney && !!searchParams && passengers.some(p => p.firstName && p.lastName && p.dateOfBirth),
    retry: 1,
  });

  const handleBackToConnections = () => {
    navigate('/connections');
  };

  const handleBackToSearch = () => {
    clearSearch();
    navigate('/');
  };

  const addPassenger = () => {
    setPassengers([...passengers, { firstName: '', lastName: '', dateOfBirth: '', discountId: null }]);
  };

  const removePassenger = (index: number) => {
    if (passengers.length > 1) {
      setPassengers(passengers.filter((_, i) => i !== index));
    }
  };

  const updatePassenger = (index: number, field: keyof Passenger, value: string | null) => {
    const newPassengers = [...passengers];
    newPassengers[index] = { ...newPassengers[index], [field]: value };
    setPassengers(newPassengers);
  };

  const handleDiscountCodeSubmit = async () => {
    if (!discountCodeInput.trim()) return;

    setIsValidatingCode(true);
    try {
      const validatedCode = await validateDiscountCode(discountCodeInput.trim());
      setDiscountCode(validatedCode);
      setDiscountCodeId(validatedCode.id);
      console.log('Discount code validated:', validatedCode);
    } catch (error) {
      console.error('Invalid discount code:', error);
      setDiscountCode(null);
      setDiscountCodeId(null);
    } finally {
      setIsValidatingCode(false);
    }
  };

  const clearDiscountCode = () => {
    setDiscountCodeInput('');
    setDiscountCode(null);
    setDiscountCodeId(null);
  };

  const handlePurchase = () => {
    // TODO: Implement ticket purchase
    console.log('Purchasing ticket with:', { passengers, discountCodeId, selectedJourney });
  };

  if (!selectedJourney || !searchParams) {
    return null; // Will redirect to main page
  }

  return (
    <div className="max-w-4xl mx-auto px-6 py-8">
      {/* Header */}
      <div className="flex items-center justify-between mb-8">
        <div className="flex items-center gap-4">
          <Button
            variant="outline"
            size="sm"
            onClick={handleBackToConnections}
            className="flex items-center gap-2"
          >
            <ArrowLeft className="h-4 w-4" />
            Back to Connections
          </Button>
          <div>
            <h1 className="text-3xl font-bold text-gray-900">Buy Ticket</h1>
            <p className="text-gray-600 mt-1">
              {selectedJourney.startStation} → {selectedJourney.endStation}
            </p>
          </div>
        </div>

        <Button
          variant="outline"
          size="sm"
          onClick={handleBackToSearch}
          className="flex items-center gap-2"
        >
          New Search
        </Button>
      </div>

      {/* Journey Summary */}
      <div className="bg-white rounded-xl shadow-lg border border-gray-200 p-6 mb-8">
        <h2 className="text-xl font-semibold text-gray-900 mb-4">Journey Summary</h2>
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 text-sm">
          <div>
            <p className="text-gray-600">Departure</p>
            <p className="font-medium">{selectedJourney.departureTime.substring(0, 5)}</p>
          </div>
          <div>
            <p className="text-gray-600">Arrival</p>
            <p className="font-medium">{selectedJourney.arrivalTime.substring(0, 5)}</p>
          </div>
          <div>
            <p className="text-gray-600">Duration</p>
            <p className="font-medium">{selectedJourney.totalDuration.substring(0, 5)}</p>
          </div>
          <div>
            <p className="text-gray-600">Transfers</p>
            <p className="font-medium">
              {selectedJourney.transfersCount === 0 ? 'Direct' : selectedJourney.transfersCount}
            </p>
          </div>
        </div>
      </div>

      {/* Passenger Form */}
      <div className="bg-white rounded-xl shadow-lg border border-gray-200 p-6 mb-8">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-xl font-semibold text-gray-900">Passengers</h2>
          <Button
            onClick={addPassenger}
            variant="outline"
            size="sm"
            className="flex items-center gap-2"
          >
            <Plus className="h-4 w-4" />
            Add Passenger
          </Button>
        </div>

        <div className="space-y-4">
          {passengers.map((passenger, index) => (
            <div key={index} className="border border-gray-200 rounded-lg p-4">
              <div className="flex items-center justify-between mb-3">
                <h3 className="font-medium text-gray-900 flex items-center gap-2">
                  <User className="h-4 w-4" />
                  Passenger {index + 1}
                </h3>
                {passengers.length > 1 && (
                  <Button
                    onClick={() => removePassenger(index)}
                    variant="outline"
                    size="sm"
                    className="text-red-600 hover:text-red-700"
                  >
                    <Trash2 className="h-4 w-4" />
                  </Button>
                )}
              </div>

              <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    First Name *
                  </label>
                  <Input
                    value={passenger.firstName}
                    onChange={(e) => updatePassenger(index, 'firstName', e.target.value)}
                    placeholder="First name"
                    required
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Last Name *
                  </label>
                  <Input
                    value={passenger.lastName}
                    onChange={(e) => updatePassenger(index, 'lastName', e.target.value)}
                    placeholder="Last name"
                    required
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Date of Birth *
                  </label>
                  <Input
                    type="date"
                    value={passenger.dateOfBirth}
                    onChange={(e) => updatePassenger(index, 'dateOfBirth', e.target.value)}
                    required
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Discount
                  </label>
                  <Select
                    value={passenger.discountId || 'none'}
                    onValueChange={(value) => updatePassenger(index, 'discountId', value === 'none' ? null : value)}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Select discount" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="none">No discount</SelectItem>
                      {isDiscountsLoading ? (
                        <SelectItem value="loading" disabled>Loading discounts...</SelectItem>
                      ) : (
                        discounts.map((discount) => (
                          <SelectItem key={discount.id} value={discount.id}>
                            {discount.name}
                          </SelectItem>
                        ))
                      )}
                    </SelectContent>
                  </Select>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Discount Code */}
      <div className="bg-white rounded-xl shadow-lg border border-gray-200 p-6 mb-8">
        <h2 className="text-xl font-semibold text-gray-900 mb-4">Discount Code (Optional)</h2>
        <div className="max-w-md">
          <Input
            value={discountCodeInput}
            onChange={(e) => setDiscountCodeInput(e.target.value)}
            placeholder="Enter discount code"
            onKeyPress={(e) => {
              if (e.key === 'Enter') {
                handleDiscountCodeSubmit();
              }
            }}
          />
          <Button
            onClick={handleDiscountCodeSubmit}
            disabled={isValidatingCode}
            className="mt-4"
            size="sm"
          >
            {isValidatingCode ? 'Validating...' : 'Apply Discount'}
          </Button>
          {discountCode && (
            <div className="mt-4 flex items-center gap-2 text-green-600">
              <Check className="h-4 w-4" />
              <span>Discount applied: {discountCode.code} ({discountCode.percentage}% off)</span>
              <Button
                onClick={clearDiscountCode}
                variant="outline"
                size="sm"
                className="text-green-600 hover:text-green-700"
              >
                <X className="h-4 w-4" />
              </Button>
            </div>
          )}
          {discountCodeInput && !discountCode && !isValidatingCode && (
            <div className="mt-4 flex items-center gap-2 text-red-600">
              <X className="h-4 w-4" />
              <span>Invalid discount code. Please try again.</span>
            </div>
          )}
        </div>
      </div>

      {/* Price Summary */}
      {ticketPrice && (
        <div className="bg-white rounded-xl shadow-lg border border-gray-200 p-6 mb-8">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">Price Summary</h2>
          <div className="space-y-3">
            <div className="flex justify-between items-center py-2">
              <span className="text-gray-600">Total Price</span>
              <span className="text-lg font-bold text-blue-600">
                {ticketPrice.currency === 'PLN' ? 'zł' : ticketPrice.currency} {ticketPrice.amount.toFixed(2)}
              </span>
            </div>
          </div>
        </div>
      )}

      {/* Purchase Button */}
      <div className="flex justify-center">
        <Button
          onClick={handlePurchase}
          disabled={!ticketPrice || isPriceLoading}
          size="lg"
          className="px-12 py-3 text-lg font-semibold bg-blue-600 hover:bg-blue-700"
        >
          {isPriceLoading ? 'Calculating Price...' : 'Purchase Ticket'}
        </Button>
      </div>

      {/* Error Display */}
      {priceError && (
        <div className="mt-6 text-center">
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
            <p className="font-bold">Error calculating price</p>
            <p>{priceError.message}</p>
            <Button
              onClick={() => refetchPrice()}
              className="mt-2"
              variant="outline"
            >
              Try Again
            </Button>
          </div>
        </div>
      )}
    </div>
  );
}

export default BuyTicket;
