import { useState, useEffect } from "react";

export function useFetch(url) {
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let active = true;

    setIsLoading(true);
    setError(null);

    fetch(url)
      .then(async (res) => {
        // HTTP Error
        if (!res.ok) {
          throw new Error("HTTP " + res.status);
        }

        // Check if response is JSON
        const contentType = res.headers.get("content-type");

        if (!contentType || !contentType.includes("application/json")) {
          throw new Error("Response is not JSON");
        }

        return res.json();
      })
      .then((json) => {
        if (active) {
          setData(json);
        }
      })
      .catch((err) => {
        if (active) {
          setError(err.message);
        }
      })
      .finally(() => {
        if (active) {
          setIsLoading(false);
        }
      });

    return () => {
      active = false;
    };
  }, [url]);

  return { data, isLoading, error };
}