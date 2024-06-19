<script lang="ts">
	import * as Breadcrumb from '$lib/components/ui/breadcrumb/index.js';
	import { page } from '$app/stores';

	function getUrlRange(url: string, start: number, end: number) {
		const parts = url.split('/').filter((part) => part !== '');
		return '/' + parts.slice(start, end + 1).join('/');
	}

	const navigation = [
		{
			path: '/tenants',
			breadcrumbs: ['Tenants']
		},
		{
			path: '/tenants/create',
			breadcrumbs: ['Tenants', 'Create']
		}
	];

	function isLastBreadcrumb(index: number): boolean {
		return index === selectedPage!.breadcrumbs.length - 1;
	}

	let selectedPage = $state<(typeof navigation)[number]>();

	$effect(() => {
		selectedPage = navigation.find((nav) => $page.url.pathname == nav.path);
	});
</script>

{#if selectedPage}
	<Breadcrumb.Root class="hidden md:flex">
		<Breadcrumb.List>
			{#each selectedPage.breadcrumbs as page, i (page)}
				<Breadcrumb.Item>
					{#if !isLastBreadcrumb(i) && selectedPage.breadcrumbs.length > 1}
						<Breadcrumb.Link href={getUrlRange(selectedPage.path, 0, i)}>
							{page}
						</Breadcrumb.Link>
					{:else}
						<Breadcrumb.Page>{page}</Breadcrumb.Page>
					{/if}
				</Breadcrumb.Item>

				{#if !isLastBreadcrumb(i)}
					<Breadcrumb.Separator />
				{/if}
			{/each}
		</Breadcrumb.List>
	</Breadcrumb.Root>
{/if}
