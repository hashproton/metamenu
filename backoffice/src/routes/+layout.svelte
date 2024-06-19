<script>
	import '../app.css';
	import { Toaster } from "$lib/components/ui/sonner";
	import Navigation from '$lib/components/layout/navigation.svelte';
</script>

<div class="flex min-h-screen w-full flex-col bg-muted/40">
	<slot />
</div>

