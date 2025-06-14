import React, {useEffect, useId, useRef, useState} from 'react';
import {MapContainer, Marker, TileLayer, useMapEvents} from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';
import {GeographicalCoordinatesDto} from '@/schemas/station-schema';





delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon-2x.png',
  iconUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-shadow.png',
});


const DEFAULT_POSITION: [number, number] = [52.2297, 21.0122];

interface MapPickerProps {
  coordinates?: GeographicalCoordinatesDto;
  onChange?: (coordinates: GeographicalCoordinatesDto) => void;
  readOnly?: boolean;
  height?: string;
  id?: string; 
}


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
  
  const uniqueId = useId();
  const mapId = id || `map-${uniqueId}`;

  
  const mapRef = useRef<L.Map | null>(null);
  const containerRef = useRef<HTMLDivElement | null>(null);

  const [position, setPosition] = useState<[number, number]>(
    coordinates ? [coordinates.latitude, coordinates.longitude] : DEFAULT_POSITION
  );

  
  const [isMounted, setIsMounted] = useState(false);

  
  const cleanupMap = () => {
    
    if (mapRef.current) {
      try {
        
        mapRef.current.remove();
        mapRef.current = null;
      } catch (e) {
        console.error('Error removing map:', e);
      }
    }

    
    try {
      
      const container = document.getElementById(mapId);
      if (container) {
        
        const mapInstance = L.DomUtil.get(mapId);
        if (mapInstance && mapInstance._leaflet_id) {
          
          mapInstance._leaflet_id = null;
        }
      }
    } catch (e) {
      console.error('Error cleaning up map container:', e);
    }
  };

  useEffect(() => {
    setIsMounted(true);

    
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

  
  useEffect(() => {
    
    cleanupMap();
    
  }, []);

  return (
    <div
      style={{height, width: '100%'}}
      id={mapId}
      ref={containerRef}
      
      key={`container-${mapId}-${Date.now()}`}
    >
      <MapContainer
        center={position}
        zoom={13}
        style={{height: '100%', width: '100%'}}
        key={`map-${mapId}-${Date.now()}`} 
        whenCreated={(map) => {
          
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
