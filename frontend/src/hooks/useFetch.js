import { useState, useEffect } from 'react';

const useFetch = (url, options) => {
    const [data, setData] = useState(null);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            if (!url) return; // Prevent fetch if URL is null

            setLoading(true);
            setError(null);

            try {
                const response = await fetch(url, options);
                const contentType = response.headers.get('content-type');

                if (contentType && contentType.includes('application/json')) {
                    const result = await response.json();

                    if (response.ok && result.statusCode === 200) {
                        setData(result.data);
                    } else {
                        throw new Error(result.statusMessage || 'Failed to fetch data');
                    }
                } else {
                    throw new Error('Unexpected content type');
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
