import React, {useEffect, useId, useRef, useState} from 'react';
import {MapContainer, Marker, TileLayer, useMapEvents} from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';
import {GeographicalCoordinatesDto} from '@/schemas/station-schema';

// Fix for Leaflet marker icons
// This is needed because Leaflet's default icon paths are based on the page location
// and when bundled with Vite, the paths are not correct
// See: https://github.com/PaulLeCam/react-leaflet/issues/453
delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon-2x.png',
  iconUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-shadow.png',
});

// Warsaw coordinates
const DEFAULT_POSITION: [number, number] = [52.2297, 21.0122];

interface MapPickerProps {
  coordinates?: GeographicalCoordinatesDto;
  onChange?: (coordinates: GeographicalCoordinatesDto) => void;
  readOnly?: boolean;
  height?: string;
  id?: string; // Optional ID for the map container
}

// Component to handle map events
function MapEvents({onChange}: { onChange?: (coordinates: GeographicalCoordinatesDto) => void }) {
  const map = useMapEvents({
    click: (e) => {
      if (onChange) {
        onChange({
          latitude: e.latlng.lat,
          longitude: e.latlng.lng,
        });
      }
    },
  });
  return null;
}

export function MapPicker({coordinates, onChange, readOnly = false, height = '400px', id}: MapPickerProps) {
  // Generate a unique ID for this instance of the map
  const uniqueId = useId();
  const mapId = id || `map-${uniqueId}`;

  // Use a ref to store the map instance
  const mapRef = useRef<L.Map | null>(null);
  const containerRef = useRef<HTMLDivElement | null>(null);

  const [position, setPosition] = useState<[number, number]>(
    coordinates ? [coordinates.latitude, coordinates.longitude] : DEFAULT_POSITION
  );

  // Track if the component is mounted to avoid state updates after unmount
  const [isMounted, setIsMounted] = useState(false);

  // Function to clean up the map instance
  const cleanupMap = () => {
    // First, check if we have a map reference
    if (mapRef.current) {
      try {
        // Remove the map instance
        mapRef.current.remove();
        mapRef.current = null;
      } catch (e) {
        console.error('Error removing map:', e);
      }
    }

    // Also try to clean up using the DOM approach
    try {
      // Find and remove any Leaflet map instances associated with this container
      const container = document.getElementById(mapId);
      if (container) {
        // Try to find any Leaflet map instances and remove them
        const mapInstance = L.DomUtil.get(mapId);
        if (mapInstance && mapInstance._leaflet_id) {
          // If there's a Leaflet instance, remove it
          mapInstance._leaflet_id = null;
        }
      }
    } catch (e) {
      console.error('Error cleaning up map container:', e);
    }
  };

  useEffect(() => {
    setIsMounted(true);

    // This cleanup function runs when the component unmounts
    return () => {
      setIsMounted(false);
      cleanupMap();
    };
  }, [mapId]);

  useEffect(() => {
    if (isMounted && coordinates) {
      setPosition([coordinates.latitude, coordinates.longitude]);
    }
  }, [coordinates, isMounted]);

  const handleMapClick = (newCoordinates: GeographicalCoordinatesDto) => {
    if (!readOnly && onChange) {
      setPosition([newCoordinates.latitude, newCoordinates.longitude]);
      onChange(newCoordinates);
    }
  };

  // Before rendering, ensure any existing map is cleaned up
  useEffect(() => {
    // Clean up before mounting to prevent "Map container is already initialized" error
    cleanupMap();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <div
      style={{height, width: '100%'}}
      id={mapId}
      ref={containerRef}
      // Add a key to the container to force re-creation
      key={`container-${mapId}-${Date.now()}`}
    >
      <MapContainer
        center={position}
        zoom={13}
        style={{height: '100%', width: '100%'}}
        key={`map-${mapId}-${Date.now()}`} // Add a unique key with timestamp
        whenCreated={(map) => {
          // Store the map instance in the ref
          mapRef.current = map;
        }}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        <Marker position={position}/>
        {!readOnly && <MapEvents onChange={handleMapClick}/>}
      </MapContainer>
    </div>
  );
}
