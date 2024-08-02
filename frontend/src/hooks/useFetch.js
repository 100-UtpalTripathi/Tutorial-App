import { useState, useEffect } from 'react';

const useFetch = (url, options = null) => {
    const [data, setData] = useState(null);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        if (!url) return; // Do nothing if URL is not provided

        const fetchData = async () => {
            setLoading(true);
            try {
                const response = await fetch(url, options);
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                const result = await response.json();
                if (result.StatusCode === 200) {
                    setData(result.Data);
                } else {
                    throw new Error(result.StatusMessage || 'Internal Server Error, Can\'t login');
                }
            } catch (err) {
                setError(err.message || 'An unexpected error occurred');
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [url, options]);

    return { data, error, loading };
};

export default useFetch;
